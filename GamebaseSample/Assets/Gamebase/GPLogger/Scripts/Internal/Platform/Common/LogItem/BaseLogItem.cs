using GamePlatform.Logger.ThirdParty;
using System;
using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public class BaseLogItem
    {
        private readonly Dictionary<string, string> logDictionary;

        public BaseLogItem()
        {
            logDictionary = new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetLogDictionary()
        {
            return logDictionary;
        }

        public string GetLogSource()
        {
            return Get(LogFields.LOG_SOURCE);
        }

        public string GetUserId()
        {
            return Get(LogFields.USER_ID);
        }

        public string GetLogVersion()
        {
            return Get(LogFields.LOG_VERSION);
        }

        public GpLogLevel GetLogLevel()
        {
            return (GpLogLevel)Enum.Parse(typeof(GpLogLevel), Get(LogFields.LOG_LEVEL));
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

        public string GetProjectKey()
        {
            return Get(LogFields.PROJECT_KEY);
        }

        public string Get(string field)
        {
            string value = string.Empty;
            logDictionary.TryGetValue(field, out value);

            return value;
        }

        public void Add(string field, string value)
        {
            if (logDictionary.ContainsKey(field) == true)
            {
                logDictionary[field] = value;
            }
            else
            {
                logDictionary.Add(field, value);
            }
        }

        public void SetUserFields(Dictionary<string, string> data)
        {
            if (data == null)
            {
                return;
            }

            var enumerator = data.GetEnumerator();
            while (enumerator.MoveNext() == true)
            {
                SetUserField(enumerator.Current.Key, enumerator.Current.Value);
            }
        }

        public void SetUserField(string field, string value)
        {
            string convertedField = LogFields.ConvertField(field);
            Add(convertedField, value);
        }

        public void SetLogSource(string logSource)
        {
            Add(LogFields.LOG_SOURCE, logSource);
        }

        public void SetDefaultLog(string projectKey, string logType, GpLogLevel logLevel, string logMessage, string transactionId = "")
        {
            Add(LogFields.PROJECT_KEY, projectKey);
            Add(LogFields.LOG_TYPE, logType);
            Add(LogFields.LOG_SOURCE, GpAppInfo.DEFAULT_LOG_SOURCE);
            Add(LogFields.LOG_VERSION, LogNCrash.VERSION);
            Add(LogFields.LOG_LEVEL, logLevel.ToString().ToUpper());
            Add(LogFields.LOG_MESSAGE, logMessage);
            Add(LogFields.LOG_CREATE_TIME, GpUtil.GetEpochMilliSeconds().ToString());
            Add(LogFields.LOG_TRANSACTION_ID, (string.IsNullOrEmpty(transactionId) == true) ? Guid.NewGuid().ToString() : transactionId);
        }

        public string GetDuplicateJsonString()
        {
            JsonData jsonData = new JsonData();
            var enumerator = logDictionary.GetEnumerator();

            while (enumerator.MoveNext() == true)
            {
                if (enumerator.Current.Key.Equals(LogFields.LOG_CREATE_TIME) == false &&
                    enumerator.Current.Key.Equals(LogFields.LOG_TRANSACTION_ID) == false)
                {
                    jsonData[enumerator.Current.Key] = enumerator.Current.Value;
                }
            }

            return jsonData.ToJson();
        }
    }
}