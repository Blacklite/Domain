using Blacklite.Framework.Domain.Process.Requests;
using Blacklite.Framework.Domain.Process.Steps;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Domain.Process
{
    public static class DomainProcessServices
    {
        public static IEnumerable<ServiceDescriptor> GetDefaultServices()
        {
            yield return ServiceDescriptor.Singleton(typeof(IDomainStepProvider), typeof(DomainStepProvider));
            yield return ServiceDescriptor.Singleton(typeof(IDomainStepCache), typeof(DomainStepCache));
            yield return ServiceDescriptor.Transient(typeof(IProcessResponse<>), typeof(ProcessResponse<>));
            yield return ServiceDescriptor.Transient(typeof(ISaveRequest<>), typeof(SaveRequest<>));
            yield return ServiceDescriptor.Transient(typeof(ISaveRequestHandler<>), typeof(SaveRequestHandler<>));
        }
    }
}
