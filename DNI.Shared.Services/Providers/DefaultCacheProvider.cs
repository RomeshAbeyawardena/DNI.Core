using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Contracts.Factories;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Providers
{
    internal sealed class DefaultCacheProvider : ICacheProvider
    {
        private readonly ICacheProviderFactory _cacheProviderFactory;

        public DefaultCacheProvider(ICacheProviderFactory cacheProviderFactory)
        {
            _cacheProviderFactory = cacheProviderFactory;
        }

        public async Task<T> Get<T>(CacheType cacheType, string cacheKeyName, 
            CancellationToken cancellationToken = default)
        {
            return await GetCacheService(cacheType)
                .Get<T>(cacheKeyName, cancellationToken);
        }

        public async Task<T> GetOrSet<T>(CacheType cacheType, string cacheKeyName, 
            Func<T> getValue, bool append = false, CancellationToken cancellationToken = default)
        {
            var value = await Get<T>(cacheType, cacheKeyName, cancellationToken);

            if(append || value == null)
                value = await Set(cacheType, cacheKeyName, getValue, cancellationToken);

            return value;
        }

        public async Task<T> GetOrSet<T>(CacheType cacheType, string cacheKeyName, 
            Func<Task<T>> getValue, bool append = false, CancellationToken cancellationToken = default)
        {
            var value = await Get<T>(cacheType, cacheKeyName, cancellationToken);

            if(append || value == null)
                value = await Set(cacheType, cacheKeyName, getValue, cancellationToken);

            return value;
        }

        public async Task Set<T>(CacheType cacheType, string cacheKeyName, T value, 
            CancellationToken cancellationToken = default)
        {
            await GetCacheService(cacheType)
                .Set(cacheKeyName, value, cancellationToken);
        }

        public async Task<T> Set<T>(CacheType cacheType, string cacheKeyName, Func<T> getValue, 
            CancellationToken cancellationToken = default)
        {
            return await GetCacheService(cacheType)
                .Set(cacheKeyName, getValue, cancellationToken);
        }

        public async Task<T> Set<T>(CacheType cacheType, string cacheKeyName, 
            Func<Task<T>> getValue, CancellationToken cancellationToken = default)
        {
            return await GetCacheService(cacheType)
                .Set(cacheKeyName, getValue, cancellationToken);
        }

        private ICacheService GetCacheService(CacheType cacheType)
        {
            return _cacheProviderFactory.GetCache(cacheType);
        }
    }
}
