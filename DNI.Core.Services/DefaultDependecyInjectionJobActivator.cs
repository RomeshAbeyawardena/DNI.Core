using Hangfire;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    internal sealed class DefaultDependecyInjectionJobActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;
        
        public DefaultDependecyInjectionJobActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new DefaultDependecyInjectionJobActivatorScope(_serviceProvider);
        }

        public override JobActivatorScope BeginScope(PerformContext context)
        {
            return new DefaultDependecyInjectionJobActivatorScope(_serviceProvider);
        }
        
        public override object ActivateJob(Type jobType)
        {
            return _serviceProvider.GetService(jobType);
        }
    }
}
