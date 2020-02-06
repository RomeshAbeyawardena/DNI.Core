using DNI.Shared.Contracts.Providers;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Providers
{
    internal sealed class DefaultClockProvider : IClockProvider
    {
        private readonly ISystemClock _systemClock;

        public DefaultClockProvider(ISystemClock systemClock)
        {
            _systemClock = systemClock;
        }

        public DateTimeOffset DateTimeOffset => _systemClock.UtcNow;

        public DateTime DateTime => DateTimeOffset.LocalDateTime;

        public DateTime UtcDateTime => _systemClock.UtcNow.UtcDateTime;
    }
}
