﻿using Blacklite.Framework;
using Blacklite.Framework.Domain.Process;
using Microsoft.Framework.ConfigurationModel;
using System;

namespace Microsoft.Framework.DependencyInjection
{
    public static class DomainProcessServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainProcesses(
            [NotNull] this IServiceCollection services,
            IConfiguration configuration = null)
        {
            services.TryAdd(DomainProcessServices.GetDefaultServices(configuration));
            return services;
        }
    }
}
