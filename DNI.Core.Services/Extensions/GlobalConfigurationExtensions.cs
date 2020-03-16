using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Extensions
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration UseDefaultDependencyInjectionActivator(this IGlobalConfiguration configuration, 
                                                                                        IServiceProvider serviceProvider)
        {
            var defaultDependencyInjectionJobActivator = serviceProvider
                .GetRequiredService<DefaultDependecyInjectionJobActivator>();
            return configuration.UseActivator(defaultDependencyInjectionJobActivator);
        }
    }
}
