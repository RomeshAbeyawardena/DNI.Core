using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Providers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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

        public static IServiceCollection RegisterCryptographicCredentials<TCryptographicCredentials>(this IServiceCollection services, 
            KeyDerivationPrf keyDerivationPrf, Encoding encoding, string password, 
            string salt, int iterations, int totalNumberOfBytes, IEnumerable<byte> initialVector)
            where TCryptographicCredentials : ICryptographicCredentials
        {
            return services.AddSingleton<ICryptographicCredentials>(serviceProvider => serviceProvider
            .GetRequiredService<ICryptographyProvider>()
            .GetCryptographicCredentials<TCryptographicCredentials>(keyDerivationPrf, encoding, password, salt,
                iterations, totalNumberOfBytes, initialVector));
        }
    }
}
