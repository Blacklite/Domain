using Blacklite.Framework.Domain.Process.Requests;
using Blacklite.Framework.Domain.Process.Steps;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Domain.Process
{
    public static class DomainProcessServices
    {
        public static IEnumerable<IServiceDescriptor> GetDefaultServices(IConfiguration configuration = null)
        {
            var describe = new ServiceDescriber(configuration);

            yield return describe.Singleton(typeof(IDomainStepProvider), typeof(DomainStepProvider));
            yield return describe.Transient(typeof(IProcessResponse<>), typeof(ProcessResponse<>));
            yield return describe.Transient(typeof(ISaveRequest<>), typeof(SaveRequest<>));
            yield return describe.Transient(typeof(ISaveRequestHandler<>), typeof(SaveRequestHandler<>));
            //yield return describe.Transient(typeof(IInitRequest<,>), typeof(InitRequest<,>));
        }
    }
}
