using DNI.Core.Contracts.Options;
using System;

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

        public bool UseJsonFileCacheEntryTrackerStore { get; private set; }

        public Func<IServiceProvider, IJsonFileCacheTrackerStoreOptions> ConfigureJsonFileCacheTrackerStoreOptions { get; private set; }
        public void RegisterJsonFileCacheTrackerStore(Action<IServiceProvider, IJsonFileCacheTrackerStoreOptions> configure)
        {
            UseJsonFileCacheEntryTrackerStore = true;
            
            ConfigureJsonFileCacheTrackerStoreOptions = (serviceProvider) =>
            {
                var options = new DefaultJsonFileCacheTrackerStoreOptions();
                configure(serviceProvider, options);
                return options;
            };
        }
    }
}
