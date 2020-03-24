namespace DNI.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    internal sealed class DefaultAppHost<TStartup> : IAppHost<TStartup>
        where TStartup : class
    {
        private readonly IServiceCollection serviceCollection;
        private Func<TStartup, IEnumerable<object>, Task> startupDelegate;

        private Task GetStartup(object[] args)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var startup = serviceProvider.GetRequiredService<TStartup>();
            return startupDelegate(startup, args);
        }

        public IAppHost<TStartup> ConfigureServices(Action<IServiceCollection> registerServices)
        {
            registerServices(serviceCollection);
            return this;
        }

        public IAppHost<TStartup> ConfigureStartupDelegate(Func<TStartup, IEnumerable<object>, Task> getStartupDelegate)
        {
            startupDelegate = getStartupDelegate;
            return this;
        }

        public async Task<T> Start<T>(params object[] args)
        {
            var startupTask = GetStartup(args);
            if (!(startupTask is Task<T> genericTask))
            {
                throw new InvalidCastException($"Unable to cast {typeof(Task)} to {typeof(Task<T>)}");
            }

            var result = await FluentTry
                .CreateAsync<T>()
                .Try(async () => await genericTask)
                .InvokeAsync().ConfigureAwait(false);

            return result.FirstOrDefault();
        }

        public async Task Start(params object[] args)
        {
            await GetStartup(args)
                .ConfigureAwait(false);
        }

        public IAppHost<TStartup> Configure(Action<IAppHost<TStartup>> configureAction)
        {
            configureAction(this);
            return this;
        }

        public DefaultAppHost()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection
                .AddSingleton(typeof(ILoggerFactory), typeof(LoggerFactory))
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
                .AddTransient<TStartup>();
        }
    }
}
