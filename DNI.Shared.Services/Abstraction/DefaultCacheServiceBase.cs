using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Services;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Abstraction
{
    public abstract class DefaultCacheServiceBase : ICacheService
    {
        private readonly IMessagePackService _messagePackService;
        internal readonly MessagePackSerializerOptions _messagePackOptions;

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
        public abstract Task<T> Set<T>(string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default);

        public DefaultCacheServiceBase(IMessagePackService messagePackService)
        {
             MessagePackSerializerOptions
                .Standard
                .WithCompression(MessagePackCompression.Lz4Block)
                .WithSecurity(MessagePackSecurity.TrustedData);
            _messagePackService = messagePackService;
        }
    }
}
