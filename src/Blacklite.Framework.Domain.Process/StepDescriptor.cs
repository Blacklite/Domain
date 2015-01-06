using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Domain.Process
{
    public interface IStepDescriptor
    {
        Func<object, IServiceProvider, HttpContext, IEnumerable<IValidation>> Execute { get; }
    }

    class StepDescriptor : IStepDescriptor
    {
        public Func<object, IServiceProvider, HttpContext, IEnumerable<IValidation>> Execute { get; private set; }

        internal IStep Step { get; private set; }

        private Type _stepType;
        private Type StepType
        {
            get
            {
                if (_stepType == null)
                    _stepType = Step.GetType();
                return _stepType;
            }
        }

        internal string Name { get { return StepType.Name; } }

        internal IEnumerable<StepStage> Stages { get; private set; }

        internal IEnumerable<StepPhase> Phases { get; private set; }

        internal Func<object, HttpContext, bool> CanExecute { get; private set; }

        internal Func<Type, bool> CanRun { get; private set; }

        internal IEnumerable<StepDescriptor> Overrides { get; private set; }

        internal IEnumerable<StepDescriptor> Before { get; private set; }

        internal IEnumerable<StepDescriptor> After { get; private set; }

        internal IEnumerable<StepDescriptor> DependsOn { get; private set; }

        internal void Fixup(IEnumerable<StepDescriptor> descriptors)
        {
            // Fixup, as they may have some null values
            Overrides = descriptors.Join(Overrides, x => x, x => x, (d, x) => d);
            Before = descriptors.Join(Before, x => x, x => x, (d, x) => d);
            After = descriptors.Join(After, x => x, x => x, (d, x) => d);

            DependsOn = After.Union(descriptors.Where(x => x.Before.Contains(this))).ToArray();
        }

        internal static StepDescriptor Create(IEnumerable<IStep> steps, ICollection<StepDescriptor> overrideSteps, IStep step)
        {
            return new StepDescriptor()
            {
                Step = step,
                Phases = GetStepPhases(step.Phase),
                Stages = GetStepStages(step.Phase),
                Overrides = GetStepOverrides(steps, overrideSteps, step),
                Before = GetRunsBefore(steps, overrideSteps, step),
                After = GetRunsAfter(steps, overrideSteps, step),
                CanExecute = GetCanExecuteAction(step),
                CanRun = GetCanRunAction(step),
                Execute = GetExecuteAction(step)
            };
        }

        private static Func<object, IServiceProvider, HttpContext, IEnumerable<IValidation>> GetExecuteAction([NotNull] IStep step)
        {
            var typeInfo = step.GetType().GetTypeInfo();

            var methodInfo = typeInfo.DeclaredMethods.FirstOrDefault(x => x.Name == "Execute" && (x.ReturnType == typeof(void) || x.ReturnType == typeof(IEnumerable<IValidation>)));
            if (methodInfo == null)
                throw new NotImplementedException(string.Format("The process step '{0}' does not implement an 'Execute' method that returns void or validation errors.", typeInfo.FullName));

            var parameterInfos = methodInfo.GetParameters();

            ParameterInfo instanceParameter = null;
            ParameterInfo httpContextParameter = null;
            ParameterInfo[] serviceParameters = null;

            return (instance, serviceProvider, httpContext) =>
            {
                // We're allowing them to stongly type the context param.
                // So we don't "know" for sure what the context param is of this step until we run once.
                if (instanceParameter == null)
                {
                    instanceParameter = parameterInfos.Single(parameterInfo => parameterInfo.ParameterType == typeof(object) || parameterInfo.ParameterType.GetTypeInfo().IsAssignableFrom(instance.GetType().GetTypeInfo()));
                    httpContextParameter = parameterInfos.SingleOrDefault(parameterInfo => typeof(HttpContext).GetTypeInfo().IsAssignableFrom(parameterInfo.ParameterType.GetTypeInfo()));
                    serviceParameters = parameterInfos.Except(new[] { instanceParameter }).ToArray();
                }

                var parameters = new object[parameterInfos.Length];
                parameters[instanceParameter.Position] = instance;
                parameters[httpContextParameter.Position] = instance;

                foreach (var parameterInfo in serviceParameters)
                {
                    try
                    {
                        parameters[parameterInfo.Position] = (httpContext?.RequestServices ?? serviceProvider).GetRequiredService(parameterInfo.ParameterType);
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format(
                            "TODO: Unable to resolve service for {0} method {1} {2}",
                            methodInfo.Name,
                            parameterInfo.Name,
                            parameterInfo.ParameterType.FullName));
                    }
                }

                if (methodInfo.ReturnType == typeof(void))
                {
                    methodInfo.Invoke(step, parameters);
                    return Enumerable.Empty<IValidation>();
                }

                return (IEnumerable<IValidation>)methodInfo.Invoke(step, parameters);
            };
        }

        private static Func<object, HttpContext, bool> GetCanExecuteAction(IStep step)
        {
            if (step is ICanExecuteStep)
            {
                return (step as ICanExecuteStep).CanExecute;
            }

            var typeInfo = step.GetType().GetTypeInfo();

            var methodInfo = typeInfo.DeclaredMethods.SingleOrDefault(x => x.Name == nameof(ICanExecuteStep.CanExecute) && x.ReturnType == typeof(bool));
            if (methodInfo == null)
                // If the step doesn't specify, then assume it always runs.
                return (httpContext, instance) => true;

            var parameterInfos = methodInfo.GetParameters();

            ParameterInfo contextParameter = null;
            ParameterInfo httpContextParameter = null;

            return (httpContext, instance) =>
            {
                // We're allowing them to stongly type the context param.
                // So we don't "know" for sure what the context param is of this step until we run once.
                if (contextParameter == null)
                {
                    contextParameter = parameterInfos.Single(parameterInfo => parameterInfo.ParameterType == typeof(object) || parameterInfo.ParameterType.GetTypeInfo().IsAssignableFrom(instance.GetType().GetTypeInfo()));
                    httpContextParameter = parameterInfos.SingleOrDefault(parameterInfo => typeof(HttpContext).GetTypeInfo().IsAssignableFrom(parameterInfo.ParameterType.GetTypeInfo()));
                }

                var parameters = new object[parameterInfos.Length];
                parameters[contextParameter.Position] = instance;
                parameters[httpContextParameter.Position] = httpContext;

                return (bool)methodInfo.Invoke(step, parameters);
            };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Code", "RedundantNameQualifierIssue", Justification = "Appears to be a mistake, when dealing with nameof.")]
        private static Func<Type, bool> GetCanRunAction(IStep step)
        {
            if (step is ICanRunStep)
            {
                return (step as ICanRunStep).CanRun;
            }

            var typeInfo = step.GetType().GetTypeInfo();

            var methodInfo = typeInfo.DeclaredMethods.SingleOrDefault(x => x.Name == nameof(ICanRunStep.CanRun) && x.ReturnType == typeof(bool));
            if (methodInfo == null)
                // If the step doesn't specify, then assume it always runs.
                return (type) => true;

            return (type) => (bool)methodInfo.Invoke(step, new object[] { type });
        }

        private static IEnumerable<StepPhase> GetStepPhases(StepPhase phase)
        {
            var phases = phase.GetFlags<StepPhase>();

            // Filter out phases that aren't real phases (just shortcuts)
            return typeof(StepPhase).GetTypeInfo().DeclaredMembers
                .Join(phases, memberInfo => memberInfo.Name, @enum => @enum.ToString(),
                    (memberInfo, x2) => new { memberInfo, x2 })
                .Where(x => x.memberInfo.GetCustomAttribute<StepStageAttribute>().Stage != StepStage.Ignore)
                .Select(x => x.x2)
                .Distinct();
        }

        private static IEnumerable<StepStage> GetStepStages(StepPhase phase)
        {
            var phases = GetStepPhases(phase);

            // Find the stages of the phases
            var memberInfos = typeof(StepPhase).GetTypeInfo().DeclaredMembers
                .Join(phases, memberInfo => memberInfo.Name, @enum => @enum.ToString(),
                    (x1, x2) => x1)
                .SelectMany(memberInfo => memberInfo.GetCustomAttributes<StepStageAttribute>())
                .Select(x => x.Stage)
                .Where(x => x != StepStage.Ignore)
                .Distinct();

            return memberInfos;
        }

        private static IEnumerable<StepDescriptor> GetStepOverrides(IEnumerable<IStep> steps, ICollection<StepDescriptor> overrideSteps, IStep step)
        {
            var stepType = step.GetType();
            foreach (var overrideStep in steps
                .Where(s => s.GetType().GetTypeInfo().IsSubclassOf(stepType)))
            {
                var descriptor = overrideSteps.SingleOrDefault(x => x.Step == overrideStep);
                if (descriptor == null)
                {
                    descriptor = Create(steps, overrideSteps, step);
                    overrideSteps.Add(descriptor);
                }
                yield return descriptor;
            }
        }

        private static IEnumerable<StepDescriptor> GetRunsAfter(IEnumerable<IStep> steps, ICollection<StepDescriptor> overrideSteps, IStep step)
        {
            foreach (var order in step.GetType()
                .GetTypeInfo()
                .GetCustomAttributes<AfterStepAttribute>())
            {
                var descriptor = overrideSteps.SingleOrDefault(x => x.StepType == order.Step);
                if (descriptor == null)
                {
                    descriptor = Create(steps, overrideSteps, steps.SingleOrDefault(x => x.GetType() == order.Step));
                    overrideSteps.Add(descriptor);
                }

                yield return descriptor;
            }
        }

        private static IEnumerable<StepDescriptor> GetRunsBefore(IEnumerable<IStep> steps, ICollection<StepDescriptor> overrideSteps, IStep step)
        {
            foreach (var order in step.GetType()
                .GetTypeInfo()
                .GetCustomAttributes<BeforeStepAttribute>())
            {
                var descriptor = overrideSteps.SingleOrDefault(x => x.StepType == order.Step);
                if (descriptor == null)
                {
                    descriptor = Create(steps, overrideSteps, steps.SingleOrDefault(x => x.GetType() == order.Step));
                    overrideSteps.Add(descriptor);
                }

                yield return descriptor;
            }
        }

        public override int GetHashCode()
        {
            return Step.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Step.Equals(((StepDescriptor)obj).Step);
        }

        public override string ToString() =>
            string.Format("Step {0} {{ Depends On: {1} }}", Name, string.Join(", ", DependsOn.Select(x => x.Name)));
    }

    static class test1
    {
        private static MethodInfo _hasFlagMethod = typeof(Enum).GetTypeInfo().DeclaredMethods.Single(x => x.Name == nameof(StepStage.HasFlag));
        public static IEnumerable<T> GetFlags<T>(this Enum value)
            where T : struct
        {
            return value.GetFlags().Cast<T>();
        }

        public static IEnumerable<Enum> GetFlags(this Enum value)
        {
            return Enum.GetValues(value.GetType())
                         .Cast<Enum>()
                         .Where(m => (bool)_hasFlagMethod.Invoke(value, new[] { m }));
        }
    }

}
