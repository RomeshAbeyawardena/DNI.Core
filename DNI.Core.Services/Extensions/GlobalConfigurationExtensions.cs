namespace DNI.Core.Services.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Hangfire;
    using Microsoft.Extensions.DependencyInjection;

    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration UseDefaultDependencyInjectionActivator(
            this IGlobalConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            var defaultDependencyInjectionJobActivator = serviceProvider
                .GetRequiredService<DefaultDependecyInjectionJobActivator>();
            return configuration.UseActivator(defaultDependencyInjectionJobActivator);
        }
    }
}
