using DNI.Core.Contracts.Options;
using System;
using System.Text.Json;

namespace DNI.Core.Services.Options
{
    internal sealed class DefaultServiceRegistrationOptions : IServiceRegistrationOptions
    {
        public static IServiceRegistrationOptions DefaultsOptions => new DefaultServiceRegistrationOptions { 
            RegisterAutoMappingProviders = true, 
            RegisterMessagePackSerialisers = true 
        };

        public bool RegisterCacheProviders { get; set; }
        public bool RegisterMessagePackSerialisers { get; set; }
        public bool RegisterAutoMappingProviders { get; set; }
        public bool RegisterMediatorServices { get; set; }
        public bool RegisterExceptionHandlers { get; set; }
        public bool RegisterCryptographicProviders { get; set; }
        
        public Func<IServiceProvider, IJsonFileCacheTrackerStoreOptions> ConfigureJsonFileCacheTrackerStoreOptions { get; private set; }
        public Func<IServiceProvider, JsonSerializerOptions> ConfigureJsonSerializerOptions { get; private set; }

        public Func<IServiceProvider, IRetryHandlerOptions> ConfigureRetryHandlerOptions { get; private set; }

        public void RegisterJsonSerializerOptions(Action<IServiceProvider, JsonSerializerOptions> configure)
        {
            ConfigureJsonSerializerOptions = (serviceProvider) =>
            {
                var jsonSerializerOptions = new JsonSerializerOptions();
                configure(serviceProvider, jsonSerializerOptions);
                return jsonSerializerOptions;
            };
        }

        public void RegisterJsonFileCacheTrackerStore(Action<IServiceProvider, IJsonFileCacheTrackerStoreOptions> configure)
        {
            ConfigureJsonFileCacheTrackerStoreOptions = (serviceProvider) =>
            {
                var options = new DefaultJsonFileCacheTrackerStoreOptions();
                configure(serviceProvider, options);
                return options;
            };
        }

        public void RegisterRetryHandlerOptions(Action<IServiceProvider, IRetryHandlerOptions> configure)
        {
            ConfigureRetryHandlerOptions = (serviceProvider) =>
            {
                var options = new DefaultRetryHandlerOptions();
                configure(serviceProvider, options);
                return options;
            };
        }
    }
}
