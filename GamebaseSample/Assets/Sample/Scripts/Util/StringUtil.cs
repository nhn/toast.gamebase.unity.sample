using UnityEngine;

namespace GamebaseSample
{
    public static class StringUtil
    {
        private static readonly string[] SIZE_SUFFIXES = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string BytesToString(long bytes)
        {
            if (bytes <= 0)
            {
                return string.Format("{0:n2}bytes", 0);
            }

            int mag = (int)Mathf.Log(bytes, 1024);
            float adjustedSize = (float)bytes / (1L << (mag * 10));

            if (Mathf.Round(adjustedSize) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n2}{1}", adjustedSize, SIZE_SUFFIXES[mag]);
        }
    }
}