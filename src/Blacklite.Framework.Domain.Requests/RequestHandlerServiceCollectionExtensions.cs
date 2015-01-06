using Blacklite.Framework;
using Blacklite.Framework.Domain.Requests;
using Microsoft.Framework.ConfigurationModel;
using System;

namespace Microsoft.Framework.DependencyInjection
{
    public static class RequestHandlerServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestHandlers(
            [NotNull] this IServiceCollection services,
            IConfiguration configuration = null)
        {
            services.TryAdd(RequestHandlerServices.GetDefaultServices(configuration));
            return services;
        }

        private static void ConfigureDefaultServices(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
