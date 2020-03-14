using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("DNI.Core.Services")]
namespace DNI.Core.Contracts.Options
{
    /// <summary>
    /// Represents a list of settings used to configure an IServiceRegistration instance
    /// </summary>
    
    public interface IServiceRegistrationOptions
    {
        /// <summary>
        /// Toggles Cache Provier support
        /// </s ummary>
        bool RegisterCacheProviders { get; set; }
        /// <summary>
        /// Toggles MessagePack serialiser support
        /// </summary>
        bool RegisterMessagePackSerialisers { get; set; }
        /// <summary>
        /// Toggles Automapper provider support
        /// </summary>
        bool RegisterAutoMappingProviders { get; set; }
        /// <summary>
        /// Toggles Mediator services support
        /// </summary>
        bool RegisterMediatorServices { get; set; }

        /// <summary>
        /// Toggles exception handler support
        /// </summary>
        bool RegisterExceptionHandlers { get; set; }

        /// <summary>
        /// Toggles Cryptographic providers
        /// </summary>
        bool RegisterCryptographicProviders { get; set; }

        public Func<IServiceProvider, IJsonFileCacheTrackerStoreOptions> ConfigureJsonFileCacheTrackerStoreOptions { get; }
        public Func<IServiceProvider, JsonSerializerOptions> ConfigureJsonSerializerOptions { get; }
        public Func<IServiceProvider, IRetryHandlerOptions> ConfigureRetryHandlerOptions { get; }
        
        void RegisterRetryHandlerOptions(Action<IServiceProvider, IRetryHandlerOptions> configure);
        void RegisterJsonFileCacheTrackerStore(Action<IServiceProvider, IJsonFileCacheTrackerStoreOptions> configure);
        void RegisterJsonSerializerOptions(Action<IServiceProvider, JsonSerializerOptions> configure);
    }
}
