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
using DNI.Shared.Domains;

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

        [Obsolete("Use overload RegisterDbContentRepositories<TDbContext>(this IServiceCollection services, " +
            "Action<DbContextRepositoryConfiguration> configure) instead.")]
        public static IServiceCollection RegisterDbContextRepositories<TDbContext>(this IServiceCollection services, 
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, 
            Action<DbContextOptionsBuilder> dbContextOptions = default,
            params Type[] entityTypes)
            where TDbContext : DbContext
        {
            return RegisterDbContentRepositories<TDbContext>(services, configuration => { 
                    configuration.ServiceLifetime = serviceLifetime;
                    configuration.DbContextOptions = dbContextOptions;
                    configuration.EntityTypes = entityTypes;
                    configuration
                        .ServiceImplementationType = typeof(DefaultEntityFrameworkRepository<,>);
                });
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
        public static IServiceCollection RegisterDbContentRepositories<TDbContext>(this IServiceCollection services, 
            Action<DbContextRepositoryConfiguration> configure)
            where TDbContext : DbContext
        {
            var configuration = new DbContextRepositoryConfiguration();
            configure(configuration);
            var serviceDefinitionType = typeof(IRepository<>);
            
            var dbContextType = typeof(TDbContext);

            if(configuration.ServiceImplementationType == null)
               configuration.ServiceImplementationType = typeof(DefaultEntityFrameworkRepository<,>);

            if(configuration.DbContextOptions !=null &&
                configuration.DbContextServiceProviderOptions !=null)
                throw new ArgumentException(
                    "Configuration must specify either DbContextOptions or DbContextServiceProviderOptions, unable to use both at the same time", nameof(DbContextOptions));

            if(configuration.DbContextOptions !=null)
                services = configuration.UseDbContextPool 
                    ? services.AddDbContextPool<TDbContext>(configuration.DbContextOptions)
                    : services.AddDbContext<TDbContext>(configuration.DbContextOptions);

            if(configuration.DbContextServiceProviderOptions !=null)
                services = configuration.UseDbContextPool 
                    ? services.AddDbContextPool<TDbContext>(configuration.DbContextServiceProviderOptions)
                    : services.AddDbContext<TDbContext>(configuration.DbContextServiceProviderOptions);


            foreach(var entityType in configuration.EntityTypes)
            {    
                var genericServiceDefinitionType = serviceDefinitionType.MakeGenericType(entityType);
                var genericServiceImplementationType = configuration.ServiceImplementationType.MakeGenericType(new [] { dbContextType, entityType });
                
                services.Add(new ServiceDescriptor(
                    genericServiceDefinitionType, 
                    genericServiceImplementationType, 
                    configuration.ServiceLifetime));
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
