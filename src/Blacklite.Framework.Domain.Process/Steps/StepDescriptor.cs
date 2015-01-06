using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Domain.Process.Steps
{
    /// <summary>
    /// Interface that describes a step.
    /// This doesn't expose the step, as each step has it's own execute method that varies.
    /// </summary>
    public interface IStepDescriptor
    {
        Func<object, IProcessContext, IEnumerable<IValidation>> Execute { get; }
    }

    /// <summary>
    /// The underlying implementation, this also has many other properties that are used
    ///    in the Step Provider to build an appropriate list of steps.
    /// </summary>
    class StepDescriptor : IStepDescriptor
    {
        /// <summary>
        ///  The execute method
        /// </summary>
        public Func<object, IProcessContext, IEnumerable<IValidation>> Execute { get; private set; }

        /// <summary>
        /// The underlying step
        /// </summary>
        public IStep Step { get; private set; }

        /// <summary>
        /// The step type, used to get the Name
        /// </summary>
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

        /// <summary>
        /// The name of the step, calls through to the type.
        /// </summary>
        public string Name { get { return StepType.FullName; } }

        /// <summary>
        /// Gets the stages this step is a part of.
        ///
        /// Stages are a special snow flake and define two (and maybe more in the future) different sets of phases.
        ///
        /// A process step can span stages
        /// </summary>
        public IEnumerable<StepStage> Stages { get; private set; }

        /// <summary>
        /// Gets the phases this step is a part of.
        ///
        /// A process step can span many phases
        /// </summary>
        public IEnumerable<StepPhase> Phases { get; private set; }

        /// <summary>
        ///  The underling Can Execute method
        /// </summary>
        public Func<object, IProcessContext, bool> CanExecute { get; private set; }

        /// <summary>
        /// The underlying Can Run method
        /// </summary>
        public Func<Type, bool> CanRun { get; private set; }

        /// <summary>
        /// If this process step has been overriden, it will be shown here.
        ///
        /// The other process step generally runs if it has been override, unless it's Can Execute method says it can't.
        /// </summary>
        public IEnumerable<StepDescriptor> Overrides { get; private set; }

        /// <summary>
        /// These are the process steps that have to run before this process step
        /// </summary>
        private IEnumerable<StepDescriptor> _before;

        /// <summary>
        /// These are the process steps that have to run after the process step
        /// </summary>
        private IEnumerable<StepDescriptor> _after;

        /// <summary>
        /// These are are all the process steps that have to run after the process, this
        ///     takes into account any process step that include us in their before section.
        ///
        /// This data is used by TopograhicalSort to produce an appropriatly sorted list.
        /// </summary>
        public IEnumerable<StepDescriptor> DependsOn { get; private set; }

        public void Fixup(IEnumerable<StepDescriptor> descriptors)
        {
            // Fixup, as they may have some null values
            Overrides = descriptors.Join(Overrides, x => x, x => x, (d, x) => d);
            _before = descriptors.Join(_before, x => x, x => x, (d, x) => d);
            _after = descriptors.Join(_after, x => x, x => x, (d, x) => d);

            DependsOn = _after.Union(descriptors.Where(x => x._before.Contains(this))).ToArray();
        }

        public static StepDescriptor Create(IEnumerable<IStep> steps, ICollection<StepDescriptor> overrideSteps, IStep step)
        {
            return new StepDescriptor()
            {
                Step = step,
                Phases = GetStepPhases(step.Phase),
                Stages = GetStepStages(step.Phase),
                Overrides = GetStepOverrides(steps, overrideSteps, step),
                _before = GetRunsBefore(steps, overrideSteps, step),
                _after = GetRunsAfter(steps, overrideSteps, step),
                CanExecute = GetCanExecuteAction(step),
                CanRun = GetCanRunAction(step),
                Execute = GetExecuteAction(step)
            };
        }

        private static Func<object, IProcessContext, IEnumerable<IValidation>> GetExecuteAction([NotNull] IStep step)
        {
            var typeInfo = step.GetType().GetTypeInfo();

            // Warn that there is no execute method.
            var methodInfo = typeInfo.DeclaredMethods.FirstOrDefault(x => x.Name == "Execute" && (x.ReturnType == typeof(void) || x.ReturnType == typeof(IEnumerable<IValidation>)));
            if (methodInfo == null)
                throw new NotImplementedException(string.Format("The process step '{0}' does not implement an 'Execute' method that returns void or validation errors.", typeInfo.FullName));

            var parameterInfos = methodInfo.GetParameters();

            ParameterInfo instanceParameter = parameterInfos.SingleOrDefault(parameterInfo => parameterInfo.ParameterType == typeof(object));
            ParameterInfo contextParameter = parameterInfos.SingleOrDefault(parameterInfo => typeof(IProcessContext).GetTypeInfo().IsAssignableFrom(parameterInfo.ParameterType.GetTypeInfo()));
            ParameterInfo[] serviceParameters = null;

            // Return our own execute method, to cache the method info in the closure.
            return (instance, context) =>
            {
                // We're allowing them to stongly type the context param.
                // So we don't "know" for sure what the context param is of this step until we run once.
                if (instanceParameter == null)
                {
                    instanceParameter = parameterInfos.Single(parameterInfo => parameterInfo.ParameterType.GetTypeInfo().IsAssignableFrom(instance.GetType().GetTypeInfo()));
                }

                if (serviceParameters == null)
                {
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

            ParameterInfo instanceParameter = parameterInfos.SingleOrDefault(parameterInfo => parameterInfo.ParameterType == typeof(object));
            ParameterInfo contextParameter = parameterInfos.SingleOrDefault(parameterInfo => typeof(IProcessContext).GetTypeInfo().IsAssignableFrom(parameterInfo.ParameterType.GetTypeInfo()));

            // Warn that this method can't be injected into.
            if (parameterInfos.Count() > 2 || (contextParameter == null && parameterInfos.Count() > 1))
                throw new NotSupportedException(string.Format("The method '{0}' is not injectable, and only supports the context parameter with an optional IProcessContext parameter.", nameof(ICanExecuteStep.CanExecute)));

            // Return our own execute method, to cache the method info in the closure.
            return (instance, context) =>
            {
                // We're allowing them to stongly type the context param.
                // So we don't "know" for sure what the context param is of this step until we run once.
                if (instanceParameter == null)
                {
                    instanceParameter = parameterInfos.Single(parameterInfo => parameterInfo.ParameterType.GetTypeInfo().IsAssignableFrom(instance.GetType().GetTypeInfo()));
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
            // Find all steps that subclass us.
            // This will add multiple steps to the list if there are mutliple children / grandchildren, this should still not be an issue.
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
            // Find all steps that run after and create their descriptor if not created.
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
            // Find all steps that run before and create their descriptor if not created.
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

        /// <summary>
        /// The descriptor is just a description of the step, and it's uniqueness is based on the step itself.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Step.GetHashCode();
        }

        /// <summary>
        /// The descriptor is just a description of the step, and it's equality is based on the step itself.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Step.Equals(((StepDescriptor)obj).Step);
        }

        // Handing for debugging.
        public override string ToString()
        {
            if (DependsOn != null && DependsOn.Any())
                return string.Format("Step {0} {{ DependsOn: {1} }}", Name, string.Join(", ", DependsOn.Select(x => x.StepType.Name)));
            return string.Format("Step {0}", Name);
        }
    }
}
