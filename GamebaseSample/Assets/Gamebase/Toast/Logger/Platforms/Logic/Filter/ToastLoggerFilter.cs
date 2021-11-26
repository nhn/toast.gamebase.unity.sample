using System.Collections.Generic;

namespace Toast.Logger
{
    public class ToastLoggerFilter
    {
        private Dictionary<string, IToastLoggerFilter> _filters = new Dictionary<string, IToastLoggerFilter>();

        public ToastLoggerFilter()
        {
            AddFilter(ToastLoggerFilterKeys.NORMAL_LOG_FILTER, new ToastLoggerNormalFilter());
            AddFilter(ToastLoggerFilterKeys.SESSION_LOG_FILTER, new ToastLoggerSessionFilter());
            AddFilter(ToastLoggerFilterKeys.CRASH_LOG_FILTER, new ToastLoggerCrashFilter());
            AddFilter(ToastLoggerFilterKeys.LOG_TYPE_FILTER, new ToastLoggerLogTypeFilter());
            AddFilter(ToastLoggerFilterKeys.LOG_LEVEL_FILTER, new ToastLoggerLogLevelFilter());
            AddFilter(ToastLoggerFilterKeys.LOG_DUPLICATE_FILTER, new ToastLoggerDuplicateFilter());
        }

        public void AddFilter(string key, IToastLoggerFilter filter)
        {
            if (_filters.ContainsKey(key))
            {
                return;
            }

            _filters.Add(key, filter);
        }

        public bool CheckFilters(ToastLoggerLogObject logObject)
        {
            string filterName = "";
            foreach (var pair in _filters)
            {
                if (!pair.Value.Filter(logObject))
                {
                    filterName = pair.Key;
                    if (ToastLoggerCommonLogic.IsLoggerListener)
                    {
                        CrashLoggerListenerReceiver.Instance.OnLogFilterWithToastLoggerObject(filterName, logObject);
                    }
                    return false;
                }
            }

            return true;
        }
    }
}