using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Services;
using DNI.Shared.Services.Abstraction;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public class DefaultDistributedCacheService : DefaultCacheServiceBase
    {
        private readonly IDistributedCache _distributedCache;

        public override async Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken)
        {
            var result = await _distributedCache.GetAsync(cacheKeyName, cancellationToken);

            if(result == null)
                return default;

            return await Deserialise<T>(result);
        }

        public override async Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default)
        {
            if(value == null)
                return;

            var serialisedValue = await Serialise(value);

            await _distributedCache.SetAsync(cacheKeyName, serialisedValue.ToArray(), cancellationToken);
        }

        public override async Task<T> Set<T>(string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default)
        {
            var value = getValue();

            if(value == null)
                return default;

            await Set(cacheKeyName, value, cancellationToken);

            return value;
        }

        public override async Task<T> Set<T>(string cacheKeyName, Func<Task<T>> getValue, CancellationToken cancellationToken = default)
        {
            var value = await getValue();

            if(value == null)
                return default;

            await Set(cacheKeyName, value, cancellationToken);

            return value;
        }

        public DefaultDistributedCacheService(IDistributedCache distributedCache, 
            IMessagePackService messagePackService)
            : base(messagePackService)
        {
            _distributedCache = distributedCache;
        }
    }
}
