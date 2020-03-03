using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Services;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Abstraction
{
    public abstract class DefaultCacheServiceBase : ICacheService
    {
        private readonly IMessagePackService _messagePackService;
        private readonly MessagePackSerializerOptions _messagePackOptions;

        protected async Task<T> Deserialise<T>(IEnumerable<byte> value)
        {
            return await _messagePackService.Deserialise<T>(value, _messagePackOptions).ConfigureAwait(false);
        }

        protected async Task<IEnumerable<byte>> Serialise<T>(T value)
        {
            return await _messagePackService.Serialise(value, _messagePackOptions).ConfigureAwait(false);
        }

        public abstract Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken = default);
        public abstract Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default);
        public abstract Task<T> Set<T>(string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default);
        public abstract Task<T> Set<T>(string cacheKeyName, Func<Task<T>> getValue, CancellationToken cancellationToken = default);

        public DefaultCacheServiceBase(IMessagePackService messagePackService)
        {
            _messagePackService = messagePackService;
            _messagePackOptions = MessagePackSerializerOptions
                .Standard
                .WithCompression(MessagePackCompression.Lz4Block)
                .WithSecurity(MessagePackSecurity.TrustedData);
        }
    }
}
