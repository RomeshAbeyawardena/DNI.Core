namespace DNI.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Hangfire;
    using Microsoft.Extensions.DependencyInjection;

    internal sealed class DefaultDependecyInjectionJobActivatorScope : JobActivatorScope
    {
        private readonly IServiceProvider serviceProvider;
        private IServiceScope serviceScope;

        public DefaultDependecyInjectionJobActivatorScope(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override object Resolve(Type type)
        {
            return (serviceScope = serviceProvider.CreateScope())
                .ServiceProvider
                .GetRequiredService(type);
        }

        public override void DisposeScope()
        {
            serviceScope?.Dispose();
        }
    }
}
