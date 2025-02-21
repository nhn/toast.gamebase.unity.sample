using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public class GpLoggerFilter
    {
        private readonly Dictionary<string, IGpLoggerFilter> filterDict;

        public GpLoggerFilter(GpLoggerResponse.LogNCrashSettings logNCrashSettings)
        {
            var result = logNCrashSettings.result;
            var filter = result.filter;

            filterDict = new Dictionary<string, IGpLoggerFilter>
            {
                { typeof(GpLoggerNormalFilter).FullName,    new GpLoggerNormalFilter(result.log.enable) },
                { typeof(GpLoggerSessionFilter).FullName,   new GpLoggerSessionFilter(result.session.enable) },
                { typeof(GpLoggerCrashFilter).FullName,     new GpLoggerCrashFilter(result.crash.enable) },
                { typeof(GpLoggerLogTypeFilter).FullName,   new GpLoggerLogTypeFilter(filter.logTypeFilter) },
                { typeof(GpLoggerLogLevelFilter).FullName,  new GpLoggerLogLevelFilter(filter.logLevelFilter) },
                { typeof(GpLoggerDuplicateFilter).FullName, new GpLoggerDuplicateFilter(filter.logDuplicateFilter) }
            };
        }

        public bool CheckFilters(BaseLogItem item)
        {
            foreach (var kvp in filterDict)
            {
                if (kvp.Value.IsLoggable(item) == false)
                {
                    var filterName = kvp.Key;

                    if (CrashLoggerReceiver.Instance != null)
                    {
                        CrashLoggerReceiver.Instance.OnLogFilterWithLogItem(filterName, item);
                    }

                    return false;
                }
            }

            return true;
        }
    }
}