namespace DNI.Core.Services.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Services;
    using MessagePack;

    public abstract class DefaultCacheServiceBase : ICacheService
    {
        private readonly IMessagePackService messagePackService;

        public DefaultCacheServiceBase(IMessagePackService messagePackService)
        {
            MessagePackOptions = MessagePackSerializerOptions
               .Standard
               .WithCompression(MessagePackCompression.Lz4Block)
               .WithSecurity(MessagePackSecurity.TrustedData);
            this.messagePackService = messagePackService;
        }

        internal MessagePackSerializerOptions MessagePackOptions { get; }

        public abstract Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken = default);

        public abstract Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default);

        public abstract Task<T> Set<T>(string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default);

        public abstract Task<T> Set<T>(string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default);

        protected async Task<T> Deserialise<T>(IEnumerable<byte> value)
        {
            return await messagePackService.Deserialise<T>(value, MessagePackOptions).ConfigureAwait(false);
        }

        protected async Task<IEnumerable<byte>> Serialise<T>(T value)
        {
            return await messagePackService.Serialise(value, MessagePackOptions).ConfigureAwait(false);
        }

    }
}
