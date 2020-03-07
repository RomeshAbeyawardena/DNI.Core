using System;

namespace DNI.Shared.Services.Extensions
{
    public static class DateTimeExtensions
    {
        public const int UnixEpochDay = 1;
        public const int UnixEpochMonth = 1;
        public const int UnixEpochYear = 1970; 
        
        public static double MillisecondsSinceUnixEpoch(this DateTimeOffset value)
        {
            var epochDateTime = new DateTimeOffset(UnixEpochYear, UnixEpochMonth, UnixEpochDay, 0, 0, 0, TimeSpan.Zero);

            return value.Subtract(epochDateTime).TotalMilliseconds;
        }
        public static double MillisecondsSinceUnixEpoch(this DateTime value)
        {
            return MillisecondsSinceUnixEpoch(new DateTimeOffset(value));
        }
    }
}
