using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Providers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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

        public static IServiceCollection RegisterDbContentRepositories<TDbContext>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, params Type[] entityTypes)
            where TDbContext : DbContext
        {
            return RegisterDbContentRepositories<TDbContext>(services, typeof(EntityFrameworkRepository<,>), serviceLifetime, entityTypes);
        }

        public static IServiceCollection RegisterDbContentRepositories<TDbContext>(this IServiceCollection services, Type serviceImplementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, params Type[] entityTypes)
            where TDbContext : DbContext
        {
            var serviceDefinitionType = typeof(IRepository<>);
            
            var dbContextType = typeof(TDbContext);

            foreach(var entityType in entityTypes)
            {
                
                var genericServiceDefinitionType = serviceDefinitionType.MakeGenericType(entityType);
                var genericServiceImplementationType = serviceImplementationType.MakeGenericType(new [] { dbContextType, entityType });
                
                services.Add(new ServiceDescriptor(
                    genericServiceDefinitionType, 
                    genericServiceImplementationType, 
                    serviceLifetime));
            }
            
            return services;
        }
    }
}
