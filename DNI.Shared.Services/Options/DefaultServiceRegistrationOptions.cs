using DNI.Shared.Contracts.Options;

namespace DNI.Shared.Services.Options
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
    }
}
