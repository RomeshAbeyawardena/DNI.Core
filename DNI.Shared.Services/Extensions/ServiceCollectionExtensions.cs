using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Options;
using DNI.Shared.Contracts.Providers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using DNI.Shared.Contracts.Generators;
using DNI.Shared.Services.Generators;
using DNI.Shared.Services.Options;

namespace DNI.Shared.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServiceBroker<TServiceBroker>(this IServiceCollection services, 
            Action<IServiceRegistrationOptions> configureOptions, out TServiceBroker serviceBrokerInstance)
            where TServiceBroker : IServiceBroker
        {
            var serviceRegistrationOptions = new DefaultServiceRegistrationOptions();
            serviceBrokerInstance = Activator
                .CreateInstance<TServiceBroker>();
            configureOptions(serviceRegistrationOptions);
            serviceBrokerInstance
                .RegisterServicesFromAssemblies(services, serviceRegistrationOptions);

            return services;
        }

        public static IServiceCollection RegisterServiceBroker<TServiceBroker, TServiceRegistrationOptions>(this IServiceCollection services, 
            Action<TServiceRegistrationOptions> configureOptions, out TServiceBroker serviceBrokerInstance)
            where TServiceBroker : IServiceBroker
            where TServiceRegistrationOptions : IServiceRegistrationOptions
        {
            var serviceRegistrationOptions = Activator
                .CreateInstance<TServiceRegistrationOptions>();
            serviceBrokerInstance = Activator
                .CreateInstance<TServiceBroker>();
            configureOptions(serviceRegistrationOptions);
            serviceBrokerInstance
                .RegisterServicesFromAssemblies(services, serviceRegistrationOptions);

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

        public static IServiceCollection RegisterDbContextRepositories<TDbContext>(this IServiceCollection services, 
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, 
            params Type[] entityTypes)
            where TDbContext : DbContext
        {
            return RegisterDbContentRepositories<TDbContext>(services, typeof(DefaultEntityFrameworkRepository<,>), serviceLifetime, entityTypes);
        }

        /// <summary>
        /// Register all classes inheriting IRepository to a Repository implementation type
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="serviceImplementationType"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="entityTypes"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterDbContentRepositories<TDbContext>(this IServiceCollection services, Type serviceImplementationType, 
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, 
            params Type[] entityTypes)
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

        public static IServiceCollection RegisterDefaultValueGenerator<TEntity>(this IServiceCollection services, Action<IDefaultValueGenerator<TEntity>> action)
        {
            return services.AddSingleton( (serviceProvider) => { 
                var defaultValueGenerator = DefaultValueGenerator<TEntity>
                    .Create(serviceProvider); 
                action(defaultValueGenerator); 
                return defaultValueGenerator; });
        }

        public static IServiceCollection RegisterCryptographicCredentialsFactory<TCryptographicCredentials>(this IServiceCollection services, Action<ISwitch<string, ICryptographicCredentials>, ICryptographyProvider, IServiceProvider> factoryBuilder)
            where TCryptographicCredentials : ICryptographicCredentials
        {
            return services.AddSingleton(services =>
            {
                var factory = Switch.Create<string, ICryptographicCredentials>();
                var cryptographyProvider = services.GetRequiredService<ICryptographyProvider>();
                factoryBuilder(factory, cryptographyProvider, services);
                return factory;
            });
        }
    }
}
