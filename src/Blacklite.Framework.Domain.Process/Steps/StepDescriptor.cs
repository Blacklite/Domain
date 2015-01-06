using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Domain.Process.Steps
{
    public interface IStepDescriptor
    {
        Func<object, IProcessContext, IEnumerable<IValidation>> Execute { get; }
    }

    class StepDescriptor : IStepDescriptor
    {
        public Func<object, IProcessContext, IEnumerable<IValidation>> Execute { get; private set; }

        public IStep Step { get; private set; }

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

        public string Name { get { return StepType.Name; } }

        public IEnumerable<StepStage> Stages { get; private set; }

        public IEnumerable<StepPhase> Phases { get; private set; }

        public Func<object, IProcessContext, bool> CanExecute { get; private set; }

        public Func<Type, bool> CanRun { get; private set; }

        public IEnumerable<StepDescriptor> Overrides { get; private set; }

        public IEnumerable<StepDescriptor> Before { get; private set; }

        public IEnumerable<StepDescriptor> After { get; private set; }

        public IEnumerable<StepDescriptor> DependsOn { get; private set; }

        public void Fixup(IEnumerable<StepDescriptor> descriptors)
        {
            // Fixup, as they may have some null values
            Overrides = descriptors.Join(Overrides, x => x, x => x, (d, x) => d);
            Before = descriptors.Join(Before, x => x, x => x, (d, x) => d);
            After = descriptors.Join(After, x => x, x => x, (d, x) => d);

            DependsOn = After.Union(descriptors.Where(x => x.Before.Contains(this))).ToArray();
        }

        public static StepDescriptor Create(IEnumerable<IStep> steps, ICollection<StepDescriptor> overrideSteps, IStep step)
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

        private static Func<object, IProcessContext, IEnumerable<IValidation>> GetExecuteAction([NotNull] IStep step)
        {
            var typeInfo = step.GetType().GetTypeInfo();

            var methodInfo = typeInfo.DeclaredMethods.FirstOrDefault(x => x.Name == "Execute" && (x.ReturnType == typeof(void) || x.ReturnType == typeof(IEnumerable<IValidation>)));
            if (methodInfo == null)
                throw new NotImplementedException(string.Format("The process step '{0}' does not implement an 'Execute' method that returns void or validation errors.", typeInfo.FullName));

            var parameterInfos = methodInfo.GetParameters();

            ParameterInfo instanceParameter = null;
            ParameterInfo contextParameter = null;
            ParameterInfo[] serviceParameters = null;

            return (instance, context) =>
            {
                // We're allowing them to stongly type the context param.
                // So we don't "know" for sure what the context param is of this step until we run once.
                if (instanceParameter == null)
                {
                    instanceParameter = parameterInfos.Single(parameterInfo => parameterInfo.ParameterType == typeof(object) || parameterInfo.ParameterType.GetTypeInfo().IsAssignableFrom(instance.GetType().GetTypeInfo()));
                    contextParameter = parameterInfos.SingleOrDefault(parameterInfo => typeof(IProcessContext).GetTypeInfo().IsAssignableFrom(parameterInfo.ParameterType.GetTypeInfo()));
                    serviceParameters = parameterInfos.Except(new[] { instanceParameter, contextParameter }).ToArray();
                }

                var parameters = new object[parameterInfos.Length];
                parameters[instanceParameter.Position] = instance;
                if (contextParameter != null)
                    parameters[contextParameter.Position] = context;

                foreach (var parameterInfo in serviceParameters)
                {
                    try
                    {
                        parameters[parameterInfo.Position] = context.ProcessServices.GetRequiredService(parameterInfo.ParameterType);
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

        private static Func<object, IProcessContext, bool> GetCanExecuteAction(IStep step)
        {
            var typeInfo = step.GetType().GetTypeInfo();

            var methodInfo = typeInfo.DeclaredMethods.SingleOrDefault(x => x.Name == nameof(ICanExecuteStep.CanExecute) && x.ReturnType == typeof(bool));
            if (methodInfo == null)
                // If the step doesn't specify, then assume it always runs.
                return (context, instance) => true;

            var parameterInfos = methodInfo.GetParameters();

            ParameterInfo instanceParameter = null;
            ParameterInfo contextParameter = parameterInfos.SingleOrDefault(parameterInfo => typeof(IProcessContext).GetTypeInfo().IsAssignableFrom(parameterInfo.ParameterType.GetTypeInfo()));

            if (parameterInfos.Count() > 2 || (contextParameter == null && parameterInfos.Count() > 1))
                throw new NotSupportedException(string.Format("The method '{0}' is not injectable, and only supports the context parameter with an optional IProcessContext parameter.", nameof(ICanExecuteStep.CanExecute)));

            return (instance, context) =>
            {
                // We're allowing them to stongly type the context param.
                // So we don't "know" for sure what the context param is of this step until we run once.
                if (instanceParameter == null)
                {
                    instanceParameter = parameterInfos.Single(parameterInfo => parameterInfo.ParameterType == typeof(object) || parameterInfo.ParameterType.GetTypeInfo().IsAssignableFrom(instance.GetType().GetTypeInfo()));
                }

                var parameters = new object[parameterInfos.Length];
                parameters[instanceParameter.Position] = instance;
                if (contextParameter != null)
                    parameters[contextParameter.Position] = context;

                return (bool)methodInfo.Invoke(step, parameters);
            };
        }

        private static Func<Type, bool> GetCanRunAction(IStep step) => step.CanRun;

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

        public override string ToString()
        {
            if (DependsOn != null && DependsOn.Any())
                return string.Format("Step {0} {{ DependsOn: {1} }}", Name, string.Join(", ", DependsOn.Select(x => x.Name)));
            return string.Format("Step {0}", Name);
        }
    }
}
