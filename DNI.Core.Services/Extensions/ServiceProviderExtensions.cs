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
        public static object CreateInjectedInstance(this IServiceProvider services, Type serviceType, params Type[] excludedTypes)
        {
            //if (serviceType.IsGenericType)
            //    serviceType = serviceType.MakeGenericType(genericParameters);

            var constructor = serviceType.GetConstructors(BindingFlags.Public).FirstOrDefault();

            var serviceList = new List<object>();

            foreach(var parameter in constructor.GetParameters())
            {
                var parameterType = parameter.ParameterType;
                if(excludedTypes.Contains(parameterType))
                    continue;
                    
                var service = services.GetService(parameterType);

                serviceList.Add(service);
            }

            return Activator.CreateInstance(serviceType, serviceList.ToArray());
        }
    }
}
