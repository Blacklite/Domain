using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Blacklite.Framework.Steps;

namespace Blacklite.Framework.Domain.Process.Steps
{
    public interface IDomainStepProvider : IPhasedStepProvider<IDomainStep, IEnumerable<IValidation>>
    {
        /// <summary>
        /// Get a grouping of all steps for the current given stage.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "stage"></param>
        /// <param name = "context"></param>
        /// <param name = "instance"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<IStepPhase, IStepDescriptor<IEnumerable<IValidation>>>> GetStepsForStage<T>(
            [NotNull] string stage,
            [NotNull] IProcessContext context,
            [NotNull] T instance)
            where T : class;
    }

    class DomainStepProvider : PhasedStepProvider<IDomainStep, IEnumerable<IValidation>>, IDomainStepProvider
    {
        private readonly IReadOnlyDictionary<string, IEnumerable<IStepPhase>> _stages;
        public DomainStepProvider(IPhasedStepCache<IDomainStep, IEnumerable<IValidation>> stepCache) : base(stepCache)
        {
            _stages = new ReadOnlyDictionary<string, IEnumerable<IStepPhase>>(stepCache.GroupBy(x => x.Key.Stage, x => x.Key).ToDictionary(x => x.Key, x => x.Distinct().OrderByDescending(z => z.Order).AsEnumerable()));
        }

        public IEnumerable<IGrouping<IStepPhase, IStepDescriptor<IEnumerable<IValidation>>>> GetStepsForStage<T>(string stage, [NotNull]
        IProcessContext context, [NotNull]
        T instance) where T : class
        {
            IEnumerable<IStepPhase> phases;
            if (!_stages.TryGetValue(stage, out phases))
                return Enumerable.Empty<IGrouping<IStepPhase, IStepDescriptor<IEnumerable<IValidation>>>>();
            return phases.Select(x => new Grouping<IStepPhase, IStepDescriptor<IEnumerable<IValidation>>>(x, GetStepsForPhase(x, context, instance)));
        }

        private class Grouping<TKey, TValue> : IGrouping<TKey, TValue>
        {
            public Grouping(TKey key, IEnumerable<TValue> values)
            {
                Key = key;
                Items = values;
            }

            public TKey Key
            {
                get;
            }

            public IEnumerable<TValue> Items
            {
                get;
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return Items.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return Items.GetEnumerator();
            }
        }
    }
}
