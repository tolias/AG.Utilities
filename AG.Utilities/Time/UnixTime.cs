using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Utilities
{
    public static class UnixTime
    {
        public static DateTime UnixTimeToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime JavaTimeToDateTime(int javaTimeStamp)
        {
            // Java timestamp is millisecods past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(javaTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static int ToUnixTime(DateTime dateTime, DateTimeKind dateTimeKind)
        {
            System.DateTime unixStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, dateTimeKind);
            TimeSpan timeSpanUnix = dateTime - unixStartTime;
            return (int)timeSpanUnix.TotalSeconds;
        }

        public static int ToJavaTime(DateTime dateTime, DateTimeKind dateTimeKind)
        {
            System.DateTime unixStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, dateTimeKind);
            TimeSpan timeSpanUnix = dateTime - unixStartTime;
            return (int)timeSpanUnix.TotalMilliseconds;
        }
    }
}
