using DNI.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal class DefaultAppHost<TStartup> : IAppHost<TStartup>
        where TStartup : class
    {
        private readonly IServiceCollection _serviceCollection;
        private Func<TStartup, IEnumerable<object>, Task> _startupDelegate;

        private Task GetStartup(object[] args)
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var startup = serviceProvider.GetRequiredService<TStartup>();
            return _startupDelegate(startup, args);
        }

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

        public async Task<T> Start<T>(params object[] args)
        {
            var startupTask = GetStartup(args);
            if (!(startupTask is Task<T> genericTask))
                throw new InvalidCastException($"Unable to cast {typeof(Task)} to Task<{typeof(T)}>");

            return await genericTask
                .ConfigureAwait(false);
        }

        public async Task Start(params object[] args)
        {
            await GetStartup(args)
                .ConfigureAwait(false);;
        }

        public DefaultAppHost()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddTransient<TStartup>();
        }
    }
}
