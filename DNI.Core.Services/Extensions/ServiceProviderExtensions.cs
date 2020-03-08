using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static TService CreateInjectedInstance(this IServiceProvider services, Type serviceType)
        {
            //if (serviceType.IsGenericType)
            //    serviceType = serviceType.MakeGenericType(genericParameters);

            var constructor = serviceType.GetConstructors(BindingFlags.Public).FirstOrDefault();

            var serviceList = new List<object>();

            foreach(var parameter in constructor.GetParameters())
            {
                serviceList.Add(services.GetService(parameter.ParameterType));
            }

            return Activator.CreateInstance(serviceType, serviceList.ToArray());
        }
    }
}
