using System;

namespace Toast.Internal
{

    public static class ToastUtil
    {
        public static long GetEpochMilliSeconds()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            return (long)t.TotalMilliseconds;
        }

    }
}