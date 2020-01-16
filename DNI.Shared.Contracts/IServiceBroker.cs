using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IServiceBroker
    {
        IEnumerable<Assembly> Assemblies { get; }
        void RegisterServicesFromAssemblies(IServiceCollection services);
    }
}
