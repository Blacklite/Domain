using Blacklite.Framework.TopographicalSort;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Domain.Process
{
    public interface IStepProvider
    {
        IEnumerable<IStepDescriptor> GetStepsForPhase<T>(StepPhase phase, [NotNull] T instance, IProcessContext context) where T : class;
        IEnumerable<KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>> GetInitSteps<T>([NotNull] T instance, IProcessContext context) where T : class;
        IEnumerable<KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>> GetSaveSteps<T>([NotNull] T instance, IProcessContext context) where T : class;
    }

    class StepProvider : IStepProvider
    {
        private readonly IReadOnlyDictionary<StepPhase, StepPhaseContainer> _steps;
        private readonly IEnumerable<KeyValuePair<StepPhase, StepPhaseContainer>> _initSteps;
        private readonly IEnumerable<KeyValuePair<StepPhase, StepPhaseContainer>> _saveSteps;

        public StepProvider(IEnumerable<IStep> allProcessSteps)
        {
            var overrideDescriptors = new List<StepDescriptor>();

            var descriptors = allProcessSteps
                .Select(step => StepDescriptor.Create(allProcessSteps, overrideDescriptors, step))
                .ToArray()
                .Except(overrideDescriptors)
                .Concat(overrideDescriptors)
                .Distinct()
                .ToArray();

            foreach (var d in descriptors)
                d.Fixup(descriptors);

            _steps = new ReadOnlyDictionary<StepPhase, StepPhaseContainer>(
                descriptors
                    .TopographicalSort(x => x.DependsOn)
                    .SelectMany(x => x.Phases, (d, p) => new { Phase = p, Descriptor = d })
                    .GroupBy(x => x.Phase, x => x.Descriptor)
                    .ToDictionary(x => x.Key, x => new StepPhaseContainer(x))
                );

            _initSteps = _steps.Where(x => (x.Key & StepPhase.InitPhases) != 0);
            _saveSteps = _steps.Where(x => (x.Key & StepPhase.SavePhases) != 0);
        }

        public IEnumerable<IStepDescriptor> GetStepsForPhase<T>(StepPhase phase, [NotNull]T instance, IProcessContext context)
            where T : class
        {
            StepPhaseContainer value;
            if (_steps.TryGetValue(phase, out value))
                return value.GetStepsForContext(instance)
                    .Where(x => x.CanExecute(instance, context));

            return Enumerable.Empty<IStepDescriptor>();
        }

        public IEnumerable<KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>> GetInitSteps<T>(
            [NotNull]T instance, IProcessContext context)
            where T : class
        {
            return _initSteps.Select(x => new KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>(
                x.Key, GetStepsForPhase(x.Key, instance, context)));
        }

        public IEnumerable<KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>> GetSaveSteps<T>(
            [NotNull]T instance, IProcessContext context)
            where T : class
        {
            return _saveSteps.Select(x => new KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>(
                x.Key, GetStepsForPhase(x.Key, instance, context)));
        }
    }
}
