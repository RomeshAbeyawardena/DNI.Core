namespace DNI.Core.Contracts.Options
{
    /// <summary>
    /// Represents a list of settings used to configure an IServiceRegistration instance
    /// </summary>
    public interface IServiceRegistrationOptions
    {
        /// <summary>
        /// Toggles Cache Provier support
        /// </summary>
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
    }
}
