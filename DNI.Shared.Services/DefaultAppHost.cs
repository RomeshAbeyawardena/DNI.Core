using DNI.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public class DefaultAppHost<TStartup> : IAppHost<TStartup>
        where TStartup : class
    {
        private readonly IServiceCollection _serviceCollection;
        private Func<TStartup, IEnumerable<object>, Task> _startupDelegate;
        public IAppHost<TStartup> ConfigureServices(Action<IServiceCollection> registerServices)
        {
            registerServices(_serviceCollection);
            return this;
        }

        public IAppHost<TStartup> ConfigureStartupDelegate(Func<TStartup, IEnumerable<object>, Task> getStartupDelegate)
        {
            _startupDelegate = getStartupDelegate;
            return this;
        }

        public async Task Start(params object[] args)
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var startup = serviceProvider.GetRequiredService<TStartup>();

            await _startupDelegate(startup, args);
        }

        public DefaultAppHost()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddTransient<TStartup>();
        }
    }
}
