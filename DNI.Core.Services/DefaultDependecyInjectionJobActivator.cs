namespace DNI.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Hangfire;
    using Hangfire.Server;

    internal sealed class DefaultDependecyInjectionJobActivator : JobActivator
    {
        private readonly IServiceProvider serviceProvider;

        public DefaultDependecyInjectionJobActivator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new DefaultDependecyInjectionJobActivatorScope(serviceProvider);
        }

        public override JobActivatorScope BeginScope(PerformContext context)
        {
            return new DefaultDependecyInjectionJobActivatorScope(serviceProvider);
        }

        public override object ActivateJob(Type jobType)
        {
            return serviceProvider.GetService(jobType);
        }
    }
}
