using Blacklite.Framework;
using Blacklite.Framework.Domain.Requests;
using System;

namespace Microsoft.Framework.DependencyInjection
{
    public static class RequestHandlerServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestHandlers([NotNull] this IServiceCollection services)
        {
            services.TryAdd(RequestHandlerServices.GetDefaultServices());
            return services;
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
        }
    }
}
