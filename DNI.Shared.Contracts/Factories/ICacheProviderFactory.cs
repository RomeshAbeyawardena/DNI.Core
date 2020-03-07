using DNI.Core.Contracts.Enumerations;
using DNI.Core.Contracts.Services;

namespace DNI.Core.Contracts.Factories
{
    public interface ICacheProviderFactory
    {
        ICacheService GetCache(CacheType cacheType);
    }
}
