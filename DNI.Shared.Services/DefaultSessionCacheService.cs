using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Services;
using DNI.Shared.Services.Abstraction;
using MessagePack;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal sealed class DefaultSessionCacheService : DefaultCacheServiceBase
    {
        private readonly ISession _session;

        public override async Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken = default)
        {
            var value = _session.Get(cacheKeyName);

            if(value == null)
                return default;

            return await Deserialise<T>(value);
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

        public override async Task<T> Set<T>(string cacheKeyName, Func<Task<T>> getValue, CancellationToken cancellationToken = default)
        {
            var value = await getValue();

            if(value == null)
                return default;

            await Set(cacheKeyName, value, cancellationToken);
            return value;
        }

        public DefaultSessionCacheService(ISession session, IMessagePackService messagePackService)
            : base(messagePackService)
        {
            _session = session;
        }
    }
}
