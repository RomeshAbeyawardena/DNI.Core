using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DNI.Core.Services
{
    internal class DefaultInstanceServiceInjector : IInstanceServiceInjector
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultInstanceServiceInjector(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TService CreateInstance<TService>()
        {
            var serviceType = typeof(TService);

            return (TService)CreateInstance(serviceType);
        }

        public object CreateInstance(Type serviceType)
        {
            var constructor = serviceType.GetConstructors().SingleOrDefault();

            var implementedServiceList = new List<object>();
            foreach (var parameter in constructor.GetParameters())
            {
                var service = _serviceProvider.GetService(parameter.ParameterType);

                if (service == null)
                    throw new ArgumentNullException(parameter.Name,
                        $"Unable to resolve service type '{ parameter.ParameterType.FullName }'");

                implementedServiceList.Add(service);
            }

            return Activator.CreateInstance(serviceType, implementedServiceList.ToArray());
        }
    }
}
