using DNI.Core.Contracts;
using DNI.Core.Contracts.Options;
using DNI.Core.Services.Abstraction;
using DNI.Core.Services.Options;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DNI.Core.Services
{

    public static class ServiceBrokerBuilder
    {
        public static IServiceBroker Build(Action<IAssembliesDescriptor> assemblyDescriptor)
        {
            return new DefaultServiceBroker(assemblyDescriptor);
        }

        public static IServiceCollection RegisterServiceBroker(IServiceCollection services, 
            IServiceBroker serviceBroker, Action<IServiceRegistrationOptions> configure)
        {
            var serviceRegistrationOptions = new DefaultServiceRegistrationOptions();
            configure(serviceRegistrationOptions);

            serviceBroker.RegisterServicesFromAssemblies(services,serviceRegistrationOptions);
            return services;
        }
    }

    internal class DefaultServiceBroker : ServiceBrokerBase
    {
        public DefaultServiceBroker(Action<IAssembliesDescriptor> assemblyDescriptor)
        {
            DescribeAssemblies = assemblyDescriptor;
        }
    }
}
