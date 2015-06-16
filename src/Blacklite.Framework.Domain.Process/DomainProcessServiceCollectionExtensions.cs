using Blacklite.Framework;
using Blacklite.Framework.Domain.Process;
using System;

namespace Microsoft.Framework.DependencyInjection
{
    public static class DomainProcessServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainProcesses([NotNull] this IServiceCollection services)
        {
            ConfigureDefaultServices(services);
            services.TryAdd(DomainProcessServices.GetDefaultServices());
            return services;
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddRequestHandlers();
        }
    }
}
