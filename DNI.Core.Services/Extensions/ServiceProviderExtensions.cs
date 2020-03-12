using DNI.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DNI.Core.Services.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static TService CreateInjectedInstance<TService>(this IServiceProvider services)
        {
            var instanceServiceInjector = services.GetRequiredService<IInstanceServiceInjector>();

            return instanceServiceInjector.CreateInstance<TService>();
        }

        public static object CreateInjectedInstance(this IServiceProvider services, Type serviceType, params Type[] excludedTypes)
        {
            //if (serviceType.IsGenericType)
            //    serviceType = serviceType.MakeGenericType(genericParameters);
            var instanceServiceInjector = services.GetRequiredService<IInstanceServiceInjector>();

            return instanceServiceInjector.CreateInstance(serviceType);
        }
    }
}
