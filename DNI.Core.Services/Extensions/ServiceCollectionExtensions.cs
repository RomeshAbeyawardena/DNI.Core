namespace DNI.Core.Services.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Generators;
    using DNI.Core.Contracts.Options;
    using DNI.Core.Contracts.Providers;
    using DNI.Core.Domains;
    using DNI.Core.Services.Generators;
    using DNI.Core.Services.Options;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServiceBroker<TServiceBroker>(
            this IServiceCollection services,
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

        public static IServiceCollection RegisterServiceBroker(
            this IServiceCollection services,
            IServiceBroker serviceBroker, Action<IServiceRegistrationOptions> configure)
        {
            return ServiceBrokerBuilder.RegisterServiceBroker(services, serviceBroker, configure);
        }

        public static IServiceCollection RegisterServiceBroker<TServiceBroker, TServiceRegistrationOptions>(
            this IServiceCollection services,
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

        public static IServiceCollection RegisterCryptographicCredentials<TCryptographicCredentials>(
            this IServiceCollection services,
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
            "Action<DbContextRepositoryConfiguration> configure) instead to enable support for future features.")]
        public static IServiceCollection RegisterDbContextRepositories<TDbContext>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
            Action<DbContextOptionsBuilder> dbContextOptions = default,
            params Type[] entityTypes)
            where TDbContext : DbContext
        {
            return RegisterDbContextRepositories<TDbContext>(services, configuration =>
            {
                foreach (var entityType in entityTypes)
                {
                    if (configuration.DescribedEntityTypes == null)
                    {
                        configuration.DescribedEntityTypes = TypesDescriptor.Describe(entityType);
                    }

                    configuration.DescribedEntityTypes.Describe(entityType);
                }

                configuration.ServiceLifetime = serviceLifetime;
                configuration.DbContextOptions = dbContextOptions;
                configuration
                    .ServiceImplementationType = typeof(DefaultEntityFrameworkRepository<,>);
            });
        }

        /// <summary>
        /// Register all classes inheriting IRepository to a Repository implementation type.
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="serviceImplementationType"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="entityTypes"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterDbContextRepositories<TDbContext>(
            this IServiceCollection services,
            Action<DbContextRepositoryConfiguration> configure)
            where TDbContext : DbContext
        {
            var configuration = new DbContextRepositoryConfiguration();
            configure(configuration);
            var serviceDefinitionType = typeof(IRepository<>);

            var dbContextType = typeof(TDbContext);

            if (configuration.ServiceImplementationType == null)
            {
                configuration.ServiceImplementationType = typeof(DefaultEntityFrameworkRepository<,>);
            }

            if (configuration.DbContextOptions != null &&
                configuration.DbContextServiceProviderOptions != null)
            {
                throw new ArgumentException(
                    "Configuration must specify either DbContextOptions or DbContextServiceProviderOptions, unable to use both at the same time", nameof(DbContextOptions));
            }

            if (configuration.DbContextOptions != null)
            {
                services = configuration.UseDbContextPool
                    ? services.AddDbContextPool<TDbContext>(configuration.DbContextOptions)
                    : services.AddDbContext<TDbContext>(configuration.DbContextOptions, configuration.ServiceLifetime);
            }

            if (configuration.DbContextServiceProviderOptions != null)
            {
                services = configuration.UseDbContextPool
                    ? services.AddDbContextPool<TDbContext>(configuration.DbContextServiceProviderOptions)
                    : services.AddDbContext<TDbContext>(configuration.DbContextServiceProviderOptions);
            }

            if (configuration.DescribedEntityTypes == null)
            {
                configuration.DescribedEntityTypes = new DefaultTypesDescriptor();
            }

            configuration.EntityTypeDescriber?.Invoke(configuration.DescribedEntityTypes);

            foreach (var entityType in configuration.EntityTypes)
            {
                var genericServiceDefinitionType = serviceDefinitionType.MakeGenericType(entityType);
                var genericServiceImplementationType = configuration.ServiceImplementationType.MakeGenericType(new[] { dbContextType, entityType });

                services.Add(new ServiceDescriptor(
                    genericServiceDefinitionType,
                    genericServiceImplementationType,
                    configuration.ServiceLifetime));
            }

            return services;
        }

        public static IServiceCollection RegisterDefaultValueGenerator<TEntity>(this IServiceCollection services, Action<IDefaultValueGenerator<TEntity>> action)
        {
            return services.AddSingleton((serviceProvider) =>
            {
                var defaultValueGenerator = DefaultValueGenerator<TEntity>
                    .Create(serviceProvider);
                action(defaultValueGenerator);
                return defaultValueGenerator;
            });
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
