using System.Collections.Generic;

namespace Toast.Core
{
    public class LogFilter
    {
        private Dictionary<string, ILogFilter> _filters = new Dictionary<string, ILogFilter>();

        public LogFilter()
        {

        }

        public void AddFilter(string key, ILogFilter filter)
        {
            if (_filters.ContainsKey(key))
            {
                return;
            }

            _filters.Add(key, filter);
        }

        public bool CheckFilters(LogObject logObject)
        {
            if (_filters.Count == 0)
            {
                return false;
            }

            foreach (var pair in _filters)
            {
                if (!pair.Value.Filter(logObject))
                {
                    return false;
                }
            }

            return true;
        }
    }
}