using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    internal sealed class DefaultDependecyInjectionJobActivatorScope : JobActivatorScope
    {
        private readonly IServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;

        public DefaultDependecyInjectionJobActivatorScope(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object Resolve(Type type)
        {
            return (_serviceScope = _serviceProvider.CreateScope())
                .ServiceProvider
                .GetRequiredService(type);
        }

        public override void DisposeScope()
        {
            _serviceScope?.Dispose();
        }
    }
}
