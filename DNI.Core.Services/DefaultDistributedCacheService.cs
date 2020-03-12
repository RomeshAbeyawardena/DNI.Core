using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Services.Abstraction;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DNI.Core.UnitTests")]
namespace DNI.Core.Services
{
    internal sealed class DefaultDistributedCacheService : DefaultCacheServiceBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ICacheEntryTracker _cacheEntryTracker;
        private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions;

        public override async Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken)
        {
            var result = await _distributedCache.GetAsync(cacheKeyName, cancellationToken).ConfigureAwait(false);
            var currentState = await _cacheEntryTracker.GetState(cacheKeyName, cancellationToken);
            
            if(result == null 
                || result.Length < 1 
                || currentState != CacheEntryState.Valid)
                return default;

            return await Deserialise<T>(result).ConfigureAwait(false);
        }

        public override async Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default)
        {
            if(value == null)
                return;

            var serialisedValue = await Serialise(value).ConfigureAwait(false);

            await _distributedCache.SetAsync(cacheKeyName, serialisedValue.ToArray(), 
                _distributedCacheEntryOptions, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<T> Set<T>(string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default)
        {
            var value = getValue();

            await Set(cacheKeyName, value, cancellationToken);

            return value;
        }

        public override async Task<T> Set<T>(string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default)
        {
            var value = await getValue(cancellationToken);

            await Set(cacheKeyName, value, cancellationToken);

            return value;
        }


        public DefaultDistributedCacheService(IDistributedCache distributedCache, ICacheEntryTracker cacheEntryTracker,
            IMessagePackService messagePackService, IOptions<DistributedCacheEntryOptions> options)
            : base(messagePackService)
        {
            _distributedCache = distributedCache;
            _cacheEntryTracker = cacheEntryTracker;
            _distributedCacheEntryOptions = options.Value;
        }
    }
}
