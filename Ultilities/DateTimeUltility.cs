using System;
namespace SignalRApi.Ultilities
{
    public class DateTimeUltility
    {
        public static long UnixTimeStamp => (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;
    }
}
