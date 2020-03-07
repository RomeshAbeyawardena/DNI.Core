namespace DNI.Shared.Contracts.Options
{
    public interface IServiceRegistrationOptions
    {
        bool RegisterCacheProviders { get; set; }
        bool RegisterMessagePackSerialisers { get; set; }
        bool RegisterAutoMappingProviders { get; set; }
        bool RegisterMediatorServices { get; set; }
        bool RegisterExceptionHandlers { get; set; }
        bool RegisterCryptographicProviders { get; set; }
    }
}
