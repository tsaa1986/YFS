using System;

namespace YFS.Core.Utilities
{
    public class UnixTimeExtension
    {
        public static class UnixTimeExtensions
        {
            public static DateTime FromUnixTimeSecondsToDateTimeUtc(long unixTime)
            {
                return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
            }
            public static long FromDateTimeUtcToUnixTimeSeconds(DateTime dateTime)
            {
                return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
            }
        }
    }
}
