using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Services.Abstraction;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

[assembly: InternalsVisibleTo("DNI.Core.UnitTests")]

namespace DNI.Core.Services
{
    internal sealed class DefaultDistributedCacheService : DefaultCacheServiceBase
    {
        private readonly IDistributedCache distributedCache;
        private readonly ICacheEntryTracker cacheEntryTracker;
        private readonly DistributedCacheEntryOptions distributedCacheEntryOptions;

        public DefaultDistributedCacheService(
            IDistributedCache distributedCache,
            ICacheEntryTracker cacheEntryTracker,
            IMessagePackService messagePackService,
            IOptions<DistributedCacheEntryOptions> options)
            : base(messagePackService)
        {
            this.distributedCache = distributedCache;
            this.cacheEntryTracker = cacheEntryTracker;
            distributedCacheEntryOptions = options.Value;
        }

        public override async Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken)
        {
            var result = await distributedCache.GetAsync(cacheKeyName, cancellationToken).ConfigureAwait(false);
            var currentState = await cacheEntryTracker.GetState(cacheKeyName, cancellationToken);

            if (result == null
                || result.Length < 1
                || currentState != CacheEntryState.Valid)
            {
                return default;
            }

            return await Deserialise<T>(result).ConfigureAwait(false);
        }

        public override async Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default)
        {
            if (value == null)
            {
                return;
            }

            var serialisedValue = await Serialise(value).ConfigureAwait(false);

            await distributedCache.SetAsync(
                cacheKeyName, 
                serialisedValue.ToArray(),
                distributedCacheEntryOptions,
                cancellationToken).ConfigureAwait(false);

            await cacheEntryTracker.SetState(cacheKeyName, CacheEntryState.Valid, cancellationToken);
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
    }
}
