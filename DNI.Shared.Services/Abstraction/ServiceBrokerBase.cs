using DNI.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Abstraction
{
    public abstract class ServiceBrokerBase : IServiceBroker
    {
        public IEnumerable<Assembly> Assemblies { get; protected set; }
        
        public static Assembly DefaultAssembly => GetAssembly<ServiceBrokerBase>();
        
        public static Assembly GetAssembly<T>()
        {
            return Assembly.GetAssembly(typeof(T));
        }

        public virtual void RegisterServicesFromAssemblies(IServiceCollection services)
        {
            foreach (var assembly in Assemblies)
            {
                var serviceRegistrationTypes = assembly
                    .GetTypes()
                    .Where(type => IsOfType<IServiceRegistration>(type));

                foreach(var serviceRegistrationType in serviceRegistrationTypes)
                    RegisterServices(serviceRegistrationType, services);
            }
        }

        private void RegisterServices(Type serviceRegistrationType, IServiceCollection services)
        {
            var serviceRegistration = (IServiceRegistration)Activator.CreateInstance(serviceRegistrationType);
            serviceRegistration.RegisterServices(services);
        }

        private bool IsOfType<T>(Type type)
        {
            var ofType = typeof(T);

            return type.GetInterface(ofType.Name) != null;
        }
    }
}
