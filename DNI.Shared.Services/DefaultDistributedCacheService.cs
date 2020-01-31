using DNI.Shared.Contracts.Services;
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
    public class DefaultDistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IMessagePackService _messagePackService;
        private MessagePackSerializerOptions _messagePackOptions;

        public async Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken)
        {
            var result = await _distributedCache.GetAsync(cacheKeyName, cancellationToken);

            if(result == null)
                return default;

            return await _messagePackService.Deserialise<T>(result, _messagePackOptions);
        }

        public async Task Set<T>(T value, CancellationToken cancellationToken = default)
        {
            if(value == null)
        }

        public Task Set<T>(Func<T> getValue, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public DefaultDistributedCacheService(IDistributedCache distributedCache, IMessagePackService messagePackService)
        {
            _distributedCache = distributedCache;
            _messagePackService = messagePackService;
        }
    }
}
