using DNI.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServiceBroker<TServiceBroker>(this IServiceCollection services)
            where TServiceBroker : IServiceBroker
        {
            var serviceBrokerInstance = Activator.CreateInstance<TServiceBroker>();

            serviceBrokerInstance.RegisterServicesFromAssemblies(services);

            return services;
        }
    }
}
