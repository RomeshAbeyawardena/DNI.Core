namespace DNI.Core.Services
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Enumerations;
    using DNI.Core.Services.Abstraction;
    using Microsoft.AspNetCore.Http;

    internal sealed class DefaultSessionCacheService : DefaultCacheServiceBase
    {
        private readonly ISession session;
        private readonly ICacheEntryTracker cacheEntryTracker;

        public override async Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken = default)
        {
            var result = session.Get(cacheKeyName);
            var currentState = await cacheEntryTracker.GetState(cacheKeyName, cancellationToken);

            if (result == null
                || result.Length < 1
                || currentState != CacheEntryState.Valid)
            {
                return default;
            }

            return await Deserialise<T>(result);
        }

        public override async Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default)
        {
            if (value == null)
            {
                return;
            }

            var serialisedValue = await Serialise(value);

            session.Set(cacheKeyName, serialisedValue.ToArray());
        }

        public override async Task<T> Set<T>(string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default)
        {
            var value = getValue();

            if (value == null)
            {
                return default;
            }

            await Set(cacheKeyName, value, cancellationToken);
            return value;
        }

        public override async Task<T> Set<T>(string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default)
        {
            var value = await getValue(cancellationToken);

            if (value == null)
            {
                return default;
            }

            await Set(cacheKeyName, value, cancellationToken);
            return value;
        }

        public DefaultSessionCacheService(
            ISession session,
            IMessagePackService messagePackService, ICacheEntryTracker cacheEntryTracker)
            : base(messagePackService)
        {
            this.session = session;
            this.cacheEntryTracker = cacheEntryTracker;
        }
    }
}
