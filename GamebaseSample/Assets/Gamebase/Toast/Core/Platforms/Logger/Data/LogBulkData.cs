using System.Collections.Generic;
using Toast.Internal;

namespace Toast.Core
{

    public class LogBulkData
    {
        private List<LogObject> _logDatas;

        public string LogContents { get; set; }
        public long CreateTime { get; set; }
        public string TransactionId { get; set; }

        public LogBulkData()
        {
            _logDatas = new List<LogObject>();
        }

        public LogBulkData(LogBulkData bulkLog)
        {
            _logDatas = new List<LogObject>(bulkLog.Gets());
        }

        public int Count
        {
            get { return _logDatas.Count; }
        }

        public void Add(List<LogObject> logDatas)
        {
            foreach (LogObject logData in logDatas)
            {
                Add(logData);
            }
        }

        public void Add(LogObject logData)
        {
            _logDatas.Add(logData);
        }

        public List<LogObject> Gets()
        {
            return _logDatas;
        }

        public string GetString()
        {
            JSONArray array = new JSONArray();
            foreach (LogObject logData in _logDatas)
            {
                array.Add(logData.GetJSONNode());
            }
            return array.ToString();
        }

        public void RemoveAll()
        {
            _logDatas.Clear();
        }
    }

}