using System;

namespace CNBot.API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int ToTimestamp(this DateTime dateTime)
        {
            var timeStart = new DateTime(1970, 1, 1);
            var ts = dateTime.ToUniversalTime() - timeStart;
            return Convert.ToInt32(ts.TotalSeconds);
        }
        public static DateTime? ToDateTime(this int timestamp)
        {
            var startTime = new DateTime(1970, 1, 1);
            startTime = startTime.AddSeconds(timestamp);
            return startTime;
        }
    }
}
