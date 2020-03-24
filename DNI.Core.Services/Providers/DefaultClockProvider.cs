namespace DNI.Core.Services.Providers
{
    using System;
    using DNI.Core.Contracts.Providers;
    using Microsoft.Extensions.Internal;

    internal sealed class DefaultClockProvider : IClockProvider
    {
        private readonly ISystemClock systemClock;

        public DefaultClockProvider(ISystemClock systemClock)
        {
            this.systemClock = systemClock;
        }

        public DateTimeOffset DateTimeOffset => systemClock.UtcNow;

        public DateTime DateTime => DateTimeOffset.LocalDateTime;

        public DateTime UtcDateTime => systemClock.UtcNow.UtcDateTime;
    }
}
