using System;
using System.Text;

namespace GamePlatform.Logger.Internal
{
    public static class GpUtil
    {
        public static long GetEpochMilliSeconds()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            return (long)t.TotalMilliseconds;
        }

        /// <summary>
        /// Merge even and odd string columns.
        /// </summary>
        /// <param name="part1">Even string columns</param>
        /// <param name="part2">Odd string columns</param>
        public static string MergeStrings(string part1, string part2)
        {
            int sizeDiff = part1.Length - part2.Length;
            if (sizeDiff < 0 || sizeDiff > 1)
            {
                throw new ArgumentException("Invalid input received");
            }

            StringBuilder sb = new StringBuilder(part1.Length + part2.Length);
            for (int i = 0; i < part1.Length; i++)
            {
                sb.Append(part1[i]);

                if (part2.Length > i)
                {
                    sb.Append(part2[i]);
                }
            }

            return sb.ToString();
        }
    }
}