using DNI.Core.Contracts;
using DNI.Core.Contracts.Options;
using DNI.Core.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DNI.Core.Services.Abstraction
{
    public abstract class ServiceBrokerBase : IServiceBroker
    {
        public Action<IAssembliesDescriptor> DescribeAssemblies { get; protected set; }

        public static Assembly GetAssembly<T>()
        {
            return Assembly.GetAssembly(typeof(T));
        }

        public IEnumerable<Assembly> Assemblies
        {
            get
            {
                var assemblyDescriptor = AssembliesDescriptor.GetAssembly<ServiceBrokerBase>();
                DescribeAssemblies(assemblyDescriptor);
                return assemblyDescriptor.Assemblies;
            }
        }

        public virtual void RegisterServicesFromAssemblies(IServiceCollection services, IServiceRegistrationOptions serviceRegistrationOptions)
        {
            foreach (var assembly in Assemblies)
            {
                var serviceRegistrationTypes = assembly
                    .GetTypes()
                    .Where(type => type.IsOfType<IServiceRegistration>());

                foreach (var serviceRegistrationType in serviceRegistrationTypes)
                    RegisterServices(serviceRegistrationType, services, serviceRegistrationOptions);

                var exceptionHandlerTypes = assembly
                    .GetTypes()
                    .Where(type => type.IsOfType<IExceptionHandler>());

                if (serviceRegistrationOptions.RegisterExceptionHandlers)
                    foreach (var exceptionHandlerType in exceptionHandlerTypes)
                        RegisterExceptionHandlers(services, exceptionHandlerType);

            }

        }

        private void RegisterServices(Type serviceRegistrationType, IServiceCollection services, IServiceRegistrationOptions serviceRegistrationOptions)
        {
            var serviceRegistration = (IServiceRegistration)Activator.CreateInstance(serviceRegistrationType);
            serviceRegistration.RegisterServices(services, serviceRegistrationOptions);
        }

        private void RegisterExceptionHandlers(IServiceCollection services, Type implementationType)
        {
            var genericServiceType = implementationType
                .GetInterfaces()
                .SingleOrDefault(interfaceType => interfaceType.IsGenericType);

            if (genericServiceType == null)
                return;

            services.AddSingleton(genericServiceType, implementationType);
        }
    }
}
