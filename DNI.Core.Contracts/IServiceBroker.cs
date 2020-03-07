using DNI.Core.Contracts.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DNI.Core.Contracts
{
    public interface IServiceBroker
    {
        IEnumerable<Assembly> Assemblies { get; }
        public Action<IAssembliesDescriptor> DescribeAssemblies { get; }
        void RegisterServicesFromAssemblies(IServiceCollection services, IServiceRegistrationOptions serviceRegistrationOptions);
    }
}
