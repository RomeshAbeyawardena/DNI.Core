using DNI.Core.Contracts.Enumerations;
using DNI.Core.Contracts.Services;

namespace DNI.Core.Contracts.Factories
{
    /// <summary>
    /// Represents a cache provider factory that returns a specific caching service
    /// </summary>
    public interface ICacheProviderFactory
    {
        /// <summary>
        /// Returns a cache service specified in cache type
        /// </summary>
        /// <param name="cacheType"></param>
        /// <returns>ICacheService</returns>
        ICacheService GetCache(CacheType cacheType);
    }
}
