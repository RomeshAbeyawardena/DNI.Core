using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IAppHost<TStartup>
    {
        Task Start(params object[] args);
        IAppHost<TStartup> ConfigureServices(Action<IServiceCollection> services);
        IAppHost<TStartup> ConfigureStartupDelegate(Func<TStartup, IEnumerable<object>, Task> getStartupDelegate);
    }
}
