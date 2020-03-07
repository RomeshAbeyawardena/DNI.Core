using System;

namespace DNI.Core.Contracts.Providers
{
    public interface IClockProvider
    {
        DateTimeOffset DateTimeOffset { get; }
        DateTime UtcDateTime { get; }
        DateTime DateTime { get; }
    }
}
