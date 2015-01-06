using Microsoft.AspNet.Http;
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
        IEnumerable<IStepDescriptor> GetStepsForPhase<T>(StepPhase phase, [NotNull] T instance, HttpContext httpContext = null) where T : class;
        IEnumerable<KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>> GetInitSteps<T>([NotNull] T instance, HttpContext httpContext = null) where T : class;
        IEnumerable<KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>> GetSaveSteps<T>([NotNull] T instance, HttpContext httpContext = null) where T : class;
    }

    class StepPhaseContainer : IEnumerable<StepDescriptor>
    {
        private class RelatedSteps
        {
            public ICollection<StepDescriptor> Before { get; } = new Collection<StepDescriptor>();
            public ICollection<StepDescriptor> After { get; } = new Collection<StepDescriptor>();
        }

        private readonly IEnumerable<StepDescriptor> _descriptors;
        private readonly ConcurrentDictionary<Type, IEnumerable<StepDescriptor>> _typeSteps = new ConcurrentDictionary<Type, IEnumerable<StepDescriptor>>();

        public StepPhaseContainer(IEnumerable<StepDescriptor> descriptors)
        {
            _descriptors = descriptors;
        }

        public IEnumerable<StepDescriptor> GetStepsForContext<T>(T context) => GetStepsForContextType(context?.GetType() ?? typeof(T));
        public IEnumerable<StepDescriptor> GetStepsForContextType(Type type) =>
            _typeSteps.GetOrAdd(type, t => _descriptors
                                // Overrides are process steps that derive from the given step
                                // If they "CanRun" then this step, by it's nature should not run.
                                // because the "override" will also be in the list of possible steps for a given type.
                                // and we should not run the same step more than once.
                                .Where(d => d.CanRun(t) && (!d.Overrides.Any() || !d.Overrides.Any(z => z.CanRun(t)))));

        public IEnumerator<StepDescriptor> GetEnumerator() => _descriptors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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

        public IEnumerable<IStepDescriptor> GetStepsForPhase<T>(StepPhase phase, [NotNull]T instance, HttpContext httpContext = null)
            where T : class
        {
            StepPhaseContainer value;
            if (_steps.TryGetValue(phase, out value))
                return value.GetStepsForContext(instance);

            return Enumerable.Empty<IStepDescriptor>();
        }

        public IEnumerable<KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>> GetInitSteps<T>(
            [NotNull]T instance, HttpContext httpContext = null)
            where T : class
        {
            return _initSteps.Select(x => new KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>(
                x.Key, x.Value.GetStepsForContext(instance)));
        }

        public IEnumerable<KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>> GetSaveSteps<T>(
            [NotNull]T instance, HttpContext httpContext = null)
            where T : class
        {
            return _saveSteps.Select(x => new KeyValuePair<StepPhase, IEnumerable<IStepDescriptor>>(
                x.Key, x.Value.GetStepsForContext(instance)));
        }
    }

    static class TopographicalSortExtensions
    {
        public static IEnumerable<T> TopographicalSort<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies)
        {
            var sorted = new List<T>();
            var visited = new HashSet<T>();

            foreach (var item in source)
                Visit(item, visited, sorted, dependencies);

            return sorted;
        }

        private static void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies)
        {
            if (!visited.Contains(item))
            {
                visited.Add(item);

                foreach (var dep in dependencies(item))
                    Visit(dep, visited, sorted, dependencies);

                sorted.Add(item);
            }
            else
            {
                if (!sorted.Contains(item))
                    throw new Exception(string.Format("Cyclic dependency found {0}", item));
            }
        }
    }
}
