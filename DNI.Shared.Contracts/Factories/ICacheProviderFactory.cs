using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Contracts.Services;

namespace DNI.Shared.Contracts.Factories
{
    public interface ICacheProviderFactory
    {
        ICacheService GetCache(CacheType cacheType);
    }
}
