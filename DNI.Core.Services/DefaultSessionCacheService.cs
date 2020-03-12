using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    internal sealed class DefaultSessionCacheService : DefaultCacheServiceBase
    {
        private readonly ISession _session;
        private readonly ICacheEntryTracker _cacheEntryTracker;

        public override async Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken = default)
        {
            var result = _session.Get(cacheKeyName);
            var currentState = await _cacheEntryTracker.GetState(cacheKeyName, cancellationToken);
            
            if(result == null 
                || result.Length < 1 
                || currentState != CacheEntryState.Valid)
                return default;

            return await Deserialise<T>(result);
        }

        public override async Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default)
        {
            if(value == null)
                return;

            var serialisedValue = await Serialise(value);

            _session.Set(cacheKeyName, serialisedValue.ToArray());
        }

        public override async Task<T> Set<T>(string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default)
        {
            var value = getValue();

            if(value == null)
                return default;

            await Set(cacheKeyName, value, cancellationToken);
            return value;
        }

        public override async Task<T> Set<T>(string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default)
        {
            var value = await getValue(cancellationToken);

            if (value == null)
                return default;

            await Set(cacheKeyName, value, cancellationToken);
            return value;
        }

        public DefaultSessionCacheService(ISession session, 
            IMessagePackService messagePackService, ICacheEntryTracker cacheEntryTracker)
            : base(messagePackService)
        {
            _session = session;
            _cacheEntryTracker = cacheEntryTracker;
        }
    }
}
