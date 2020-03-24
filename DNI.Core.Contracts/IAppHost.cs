namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public interface IAppHost<TStartup>
    {
        Task Start(params object[] args);

        Task<T> Start<T>(params object[] args);

        IAppHost<TStartup> ConfigureServices(Action<IServiceCollection> services);

        IAppHost<TStartup> ConfigureStartupDelegate(Func<TStartup, IEnumerable<object>, Task> getStartupDelegate);

        IAppHost<TStartup> Configure(Action<IAppHost<TStartup>> configureAction);
    }
}
