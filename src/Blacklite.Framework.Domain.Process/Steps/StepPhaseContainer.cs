using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blacklite.Framework.Domain.Process.Steps
{
    /// <summary>
    /// Contains a cache of step descriptors, for any type that has come through for the phase that we're constructed for
    /// We don't know our phase (nor do we really care about it.)
    ///
    /// We also implement the enumerator to make it easy to list all steps in a phase.
    /// </summary>
    class StepPhaseContainer : IEnumerable<StepDescriptor>
    {
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
}
