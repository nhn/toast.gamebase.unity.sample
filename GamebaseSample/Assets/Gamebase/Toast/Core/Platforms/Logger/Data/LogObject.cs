using System;
using System.Collections.Generic;
using Toast.Internal;

namespace Toast.Core
{

    public class LogObject
    {
        private Dictionary<string, string> _data = new Dictionary<string, string>();

        public string GetLogSource()
        {
            return Get(LogFields.LOG_SOURCE);
        }

        public void SetUserId(string userId)
        {
            if (ContainsFiled(LogFields.USER_ID))
            {
                Remove(LogFields.USER_ID);
            }

            Put(LogFields.USER_ID, userId);
        }

        public string GetUserId()
        {
            return Get(LogFields.USER_ID);
        }

        public string GetLogVersion()
        {
            return Get(LogFields.LOG_VERSION);
        }

        public LogLevel GetLogLevel()
        {
            return (LogLevel)Enum.Parse(typeof(LogLevel), Get(LogFields.LOG_LEVEL));
        }

        public string GetLoggerType()
        {
            return Get(LogFields.LOG_TYPE);
        }

        public string GetLogMessage()
        {
            return Get(LogFields.LOG_MESSAGE);
        }

        public long GetCreateTime()
        {
            return long.Parse(Get(LogFields.LOG_CREATE_TIME));
        }

        public string GetTransactionId()
        {
            return Get(LogFields.LOG_TRANSACTION_ID);
        }

        public bool ContainsFiled(string field)
        {
            return _data.ContainsKey(field);
        }

        public string Get(string field)
        {
            return _data[field];
        }

        public Dictionary<string, string> Gets()
        {
            return _data;
        }

        public void Put(string field, string value)
        {
            _data.Add(field, value);
        }

        private void Remove(string field)
        {
            _data.Remove(field);
        }

        public void PutAll(Dictionary<string, string> data)
        {
            var enumerator = data.GetEnumerator();
            while (enumerator.MoveNext())
            {
                _data.Add(enumerator.Current.Key, enumerator.Current.Value);
            }
        }

        public void SetUserField(string field, string value)
        {
            string convertedField = LogFields.ConvertField(field);

            if (ContainsFiled(convertedField))
            {
                Remove(convertedField);
            }

            Put(convertedField, value);
        }

        public void SetUserFields(Dictionary<string, string> data)
        {
            var enumerator = data.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SetUserField(enumerator.Current.Key, enumerator.Current.Value);
            }
        }

        public string GetProjectKey()
        {
            return Get(LogFields.PROJECT_KEY);
        }

        public void SetLogObject(string projectKey, string logType, LogLevel logLevel, string logMessage, string transactionId = "")
        {
            _data.Add(LogFields.PROJECT_KEY, projectKey);
            _data.Add(LogFields.LOG_TYPE, logType);
            _data.Add(LogFields.LOG_SOURCE, LogConstants.DEFAULT_LOG_SOURCE);
            _data.Add(LogFields.LOG_VERSION, ApiVersion.V2);
            _data.Add(LogFields.LOG_LEVEL, logLevel.ToString());
            _data.Add(LogFields.LOG_MESSAGE, logMessage);
            _data.Add(LogFields.LOG_CREATE_TIME, ToastUtil.GetEpochMilliSeconds().ToString());
            _data.Add(LogFields.LOG_TRANSACTION_ID, (transactionId == "") ? Guid.NewGuid().ToString() : transactionId);
        }

        public string GetJSONString()
        {
            JSONNode node = new JSONObject();
            foreach (var parameter in _data)
            {
                node.Add(parameter.Key, parameter.Value);
            }

            return node.ToString();
        }

        public JSONNode GetJSONNode()
        {
            JSONNode node = new JSONObject();
            foreach (var parameter in _data)
            {
                node.Add(parameter.Key, parameter.Value);
            }

            return node;
        }
    }

}
