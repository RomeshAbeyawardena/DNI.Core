using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IAppHost<TStartup>
    {
        Task Start(params object[] args);
        Task<T> Start<T>(params object[] args);
        IAppHost<TStartup> ConfigureServices(Action<IServiceCollection> services);
        IAppHost<TStartup> ConfigureStartupDelegate(Func<TStartup, IEnumerable<object>, Task> getStartupDelegate);
        IAppHost<TStartup> Configure(Action<IAppHost<TStartup>> configureAction);
    }
}
