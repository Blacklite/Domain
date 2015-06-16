using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Domain.Requests
{
    public static class RequestHandlerServices
    {
        public static IEnumerable<ServiceDescriptor> GetDefaultServices()
        {
            yield return ServiceDescriptor.Transient(typeof(IRequestHandler<,>), typeof(RequestHandler<,>));
        }
    }
}
