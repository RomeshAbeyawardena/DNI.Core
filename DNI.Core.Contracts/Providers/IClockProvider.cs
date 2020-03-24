namespace DNI.Core.Contracts.Providers
{
    using System;

    /// <summary>
    /// Represents a clock provider to aid testing against the system clock
    /// using ISystemClock.
    /// </summary>
    public interface IClockProvider
    {
        /// <summary>
        /// Returns a DateTimeOffset instance of ISystemClock.Now.
        /// </summary>
        DateTimeOffset DateTimeOffset { get; }

        /// <summary>
        /// Returns a DateTime instance of ISystemClock.UtcNow.
        /// </summary>
        DateTime UtcDateTime { get; }

        /// <summary>
        /// Returns a DateTime instance of ISystemClock.Now.Date.
        /// </summary>
        DateTime DateTime { get; }
    }
}
