using DNI.Shared.Contracts;
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
        private readonly IIs _is;
        private readonly ICacheProviderFactory _cacheProviderFactory;

        public DefaultCacheProvider(IIs @is, ICacheProviderFactory cacheProviderFactory)
        {
            _is = @is;
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

        public async Task<IEnumerable<T>> GetOrSet<T>(CacheType cacheType, string cacheKeyName, 
            Func<Task<IEnumerable<T>>> getValue, 
            Func<T, object> IdSelector, Func<Task<object>> getMaxValue, bool append = false, 
            CancellationToken cancellationToken = default)
        {
            var value = await Get<IEnumerable<T>>(cacheType, cacheKeyName, cancellationToken);

            if(value == null)
                return await Set(cacheType, cacheKeyName, getValue, cancellationToken);

            var currentValue = await getMaxValue();
            var lastValue = value.LastOrDefault();

            if(lastValue == null)
                return await Set(cacheType, cacheKeyName, getValue, cancellationToken);

            var idValue = IdSelector(lastValue);

            bool outDated = true; 
            
            var currentValType = _is
                .TryDetermineType(currentValue, out var currentVal);
            var idType = _is
                .TryDetermineType(idValue, out var idVal);

            if(currentValType != OfType.String 
                && idType != OfType.String)
                outDated = currentVal > idVal;

            if(outDated)
                return await Set(cacheType, cacheKeyName, getValue, cancellationToken);

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
