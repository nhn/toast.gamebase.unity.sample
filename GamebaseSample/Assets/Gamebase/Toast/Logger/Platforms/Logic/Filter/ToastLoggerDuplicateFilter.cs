using System.Collections.Generic;
using Toast.Internal;

namespace Toast.Logger
{
    public class ToastLoggerDuplicateFilter : IToastLoggerFilter
    {
        private const int DEFAULT_DUPLICATE_FILTER_CAPACITY = 2048;
        private const long DEFAULT_DUPLICATE_FILTER_EXPIRE_TIME_MILLIS = 2000L;
        private List<ToastLoggerDuplicateInfo> _compareDuplicateInfos = new List<ToastLoggerDuplicateInfo>();

        private int _capacity = DEFAULT_DUPLICATE_FILTER_CAPACITY;

        public void SetCapacity(int capacity)
        {
            _capacity = capacity;
        }

        public bool Filter(ToastLoggerLogObject logData)
        {
            if (ToastLoggerSettings.Instance.isLogDuplicateFilter == false)
            {
                return true;
            }

            long currentTime = ToastUtil.GetEpochMilliSeconds();

            _compareDuplicateInfos.RemoveAll(info => info.CreateTime < (currentTime - ToastLoggerSettings.Instance.filterDuplicateLogExpiredTimeSeconds * 1000));

            ToastLoggerDuplicateInfo duplicateInfo = new ToastLoggerDuplicateInfo();
            duplicateInfo.SetLogData(logData);

            foreach (var item in _compareDuplicateInfos)
            {
                if (item.Key == duplicateInfo.Key)
                {
                    return false;
                }
            }

            if (_compareDuplicateInfos.Count >= _capacity)
            {
                _compareDuplicateInfos.RemoveAt(0);
            }

            _compareDuplicateInfos.Add(duplicateInfo);

            return true;
        }
    }
}
