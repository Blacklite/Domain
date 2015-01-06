using Blacklite.Framework.TopographicalSort;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Domain.Process.Steps
{
    /// <summary>
    /// Provides a list of steps for a phase, or an overall stage.
    /// </summary>
    public interface IStepProvider
    {
        /// <summary>
        /// Get a list of all steps for the given phase
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="phase"></param>
        /// <param name="instance"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        IEnumerable<IStepDescriptor> GetStepsForPhase<T>(StepPhase phase, [NotNull] T instance, IProcessContext context) where T : class;
        /// <summary>
        /// Get a grouping of all the steps for the init stage
        ///
        /// This list is ordered based on the run order of the stage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<StepPhase, IStepDescriptor>> GetInitSteps<T>([NotNull] T instance, IProcessContext context) where T : class;
        /// <summary>
        /// Get a grouping of all the steps for the save stage
        ///
        /// This list is ordered based on the run order of the stage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<StepPhase, IStepDescriptor>> GetSaveSteps<T>([NotNull] T instance, IProcessContext context) where T : class;
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
                // Create the desciptor
                .Select(step => StepDescriptor.Create(allProcessSteps, overrideDescriptors, step))
                .ToArray()
                // Exclude anyones that are part of depdendencies or overrides
                .Except(overrideDescriptors)
                // Include the ones that were added as part of creation
                .Concat(overrideDescriptors)
                // Make sure we only have a unique list (one per step)
                .Distinct()
                .ToArray();

            if (allProcessSteps.Count() != descriptors.Count())
                throw new IndexOutOfRangeException();

            // Fixup all the descriptors to normalize before / after.
            foreach (var d in descriptors)
                d.Fixup(descriptors);

            _steps = new ReadOnlyDictionary<StepPhase, StepPhaseContainer>(
                descriptors
                    // Sorts the entire list based on the dependencies.
                    .TopographicalSort(x => x.DependsOn)
                    // Select all the descitptors out into each of the supported phases
                    .SelectMany(x => x.Phases, (d, p) => new { Phase = p, Descriptor = d })
                    // Group for each phase
                    .GroupBy(x => x.Phase, x => x.Descriptor)
                    // Place each phase in a container.
                    .ToDictionary(x => x.Key, x => new StepPhaseContainer(x))
                );

            // Isolate major stages for later use.
            _initSteps = _steps.Where(x => (x.Key & StepPhase.InitPhases) != 0);
            _saveSteps = _steps.Where(x => (x.Key & StepPhase.SavePhases) != 0);
        }

        private class Grouping<TKey, TValue> : IGrouping<TKey, TValue>
        {
            public Grouping(TKey key, IEnumerable<TValue> values)
            {
                Key = key;
                Items = values;
            }
            public TKey Key { get; }
            public IEnumerable<TValue> Items { get; }
            public IEnumerator<TValue> GetEnumerator()
            {
                return Items.GetEnumerator();
            }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return Items.GetEnumerator();
            }
        }

        public IEnumerable<IStepDescriptor> GetStepsForPhase<T>(StepPhase phase, [NotNull]T instance, IProcessContext context)
            where T : class
        {
            StepPhaseContainer value;
            if (_steps.TryGetValue(phase, out value))
                return value.GetStepsForContext(instance)
                    // Overrides are process steps that derive from the given step
                    // If they "CanExecute" then this step, by it's nature should not execute.
                    // because the "override" will also be in the list of possible steps for a given type.
                    // and we should not run the same step more than once.
                    .Where(step => step.CanExecute(instance, context) && (!step.Overrides.Any() || !step.Overrides.Any(z => z.CanExecute(instance, context))));

            return Enumerable.Empty<IStepDescriptor>();
        }

        public IEnumerable<IGrouping<StepPhase, IStepDescriptor>> GetInitSteps<T>(
            [NotNull]T instance, IProcessContext context)
            where T : class
        {
            return _initSteps.Select(x => new Grouping<StepPhase, IStepDescriptor>(x.Key, GetStepsForPhase(x.Key, instance, context)));
        }

        public IEnumerable<IGrouping<StepPhase, IStepDescriptor>> GetSaveSteps<T>(
            [NotNull]T instance, IProcessContext context)
            where T : class
        {
            return _saveSteps.Select(x => new Grouping<StepPhase, IStepDescriptor>(x.Key, GetStepsForPhase(x.Key, instance, context)));
        }
    }
}
