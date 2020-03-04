using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Contracts.Factories;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DNI.Shared.UnitTests")]
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

        public async Task<T> Get<T>(CacheType cacheType, string cacheKeyName, CancellationToken cancellationToken = default)
        {
            var cacheService = GetCacheService(cacheType);

            return await cacheService.Get<T>(cacheKeyName, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetOrSet<T>(CacheType cacheType, string cacheKeyName, 
            Func<CancellationToken, Task<IEnumerable<T>>> getValue, 
            Func<T, object> selectId, Func<CancellationToken, Task<object>> getMaxValue, bool append = false, 
            CancellationToken cancellationToken = default)
        {
            var value = await Get<IEnumerable<T>>(cacheType, cacheKeyName, cancellationToken);

            if(value == null || !value.Any())
                return await Set(cacheType, cacheKeyName, async (cancellationToken) => await getValue(cancellationToken));

            var currentValue = await getMaxValue(cancellationToken);
            var lastValue = value.LastOrDefault();

            var identifierValue = selectId(lastValue);

            var isOutdated = false;

            var currentValType = _is
                .TryDetermineType(currentValue, out var currentVal);
            var idType = _is
                .TryDetermineType(identifierValue, out var idVal);

            if (currentValType != OfType.String
                && idType != OfType.String)
                isOutdated = currentVal > idVal;

            if (isOutdated)
                return await Set(cacheType, cacheKeyName, getValue, cancellationToken);

            return value;
        }

        public async Task<T> GetOrSet<T>(CacheType cacheType, string cacheKeyName, Func<CancellationToken, Task<T>> getValue, bool append = false, CancellationToken cancellationToken = default)
        {
            var value = await Get<T>(cacheType, cacheKeyName, cancellationToken);

            if(value == null)
                return await Set(cacheType, cacheKeyName, 
                    async(cancellationToken) => await getValue(cancellationToken), cancellationToken);

            return value;
        }

        public async Task Set<T>(CacheType cacheType, string cacheKeyName, T value, CancellationToken cancellationToken = default)
        {
            var cacheService = GetCacheService(cacheType);
            await cacheService.Set(cacheKeyName, value, cancellationToken);

        }

        public async Task<T> Set<T>(CacheType cacheType, string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default)
        {
            var cacheService = GetCacheService(cacheType);
            return await cacheService.Set(cacheKeyName, getValue, cancellationToken);
        }

        public async Task<T> Set<T>(CacheType cacheType, string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default)
        {
            var cacheService = GetCacheService(cacheType);
            return await cacheService.Set(cacheKeyName, getValue, cancellationToken);

        }

        private ICacheService GetCacheService(CacheType cacheType)
        {
            return _cacheProviderFactory.GetCache(cacheType);
        }
    }
}
