namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using DNI.Core.Contracts.Options;
    using Microsoft.Extensions.DependencyInjection;

    public interface IServiceBroker
    {
        IEnumerable<Assembly> Assemblies { get; }

        public Action<IAssembliesDescriptor> DescribeAssemblies { get; }

        void RegisterServicesFromAssemblies(IServiceCollection services, IServiceRegistrationOptions serviceRegistrationOptions);
    }
}
