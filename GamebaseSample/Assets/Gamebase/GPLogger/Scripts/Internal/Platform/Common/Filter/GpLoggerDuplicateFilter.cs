using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public class GpLoggerDuplicateFilter : IGpLoggerFilter
    {
        private const int DEFAULT_DUPLICATE_FILTER_CAPACITY = 2048;

        private readonly GpLoggerResponse.LogNCrashSettings.Result.Filter.LogDuplicateFilter filter;

        private List<GpLoggerDuplicateInfo> compareDuplicateInfos = new List<GpLoggerDuplicateInfo>();

        public GpLoggerDuplicateFilter(GpLoggerResponse.LogNCrashSettings.Result.Filter.LogDuplicateFilter filter)
        {
            this.filter = filter;
        }

        public bool IsLoggable(BaseLogItem item)
        {
            if (filter.enable == false)
            {
                return true;
            }

            var currentTime = GpUtil.GetEpochMilliSeconds();

            compareDuplicateInfos.RemoveAll(info => info.CreateTime < (currentTime - filter.expiredTime * 1000));

            var duplicateInfo = new GpLoggerDuplicateInfo();
            duplicateInfo.SetLogItem(item);

            foreach (var info in compareDuplicateInfos)
            {
                if (info.Key.Equals(duplicateInfo.Key) == true)
                {
                    return false;
                }
            }

            if (compareDuplicateInfos.Count >= DEFAULT_DUPLICATE_FILTER_CAPACITY)
            {
                compareDuplicateInfos.RemoveAt(0);
            }

            compareDuplicateInfos.Add(duplicateInfo);

            return true;
        }
    }
}
