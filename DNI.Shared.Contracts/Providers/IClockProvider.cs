using System;

namespace DNI.Shared.Contracts.Providers
{
    public interface IClockProvider
    {
        DateTimeOffset DateTimeOffset { get; }
        DateTime UtcDateTime { get; }
        DateTime DateTime { get; }
    }
}
