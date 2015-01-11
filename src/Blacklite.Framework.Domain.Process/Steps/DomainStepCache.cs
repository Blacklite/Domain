using Blacklite.Framework.TopographicalSort;
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
    public interface IDomainStepCache : IPhasedStepCache<IDomainStep, IEnumerable<IValidation>>
    {

    }

    public class DomainStepCache : PhasedStepCache<IDomainStep, IEnumerable<IValidation>>, IDomainStepCache
    {
        public DomainStepCache(IEnumerable<IDomainStep> steps) : base(steps)
        {
        }
    }
}
