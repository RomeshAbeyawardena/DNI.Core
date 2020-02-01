using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace DNI.Shared.Contracts
{
    public interface IServiceBroker
    {
        IEnumerable<Assembly> Assemblies { get; }
        void RegisterServicesFromAssemblies(IServiceCollection services, IServiceRegistrationOptions serviceRegistrationOptions);
    }
}
