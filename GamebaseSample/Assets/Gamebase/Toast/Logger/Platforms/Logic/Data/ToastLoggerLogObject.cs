using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Toast.Internal;

namespace Toast.Logger
{
    public class ToastLoggerLogObject
    {
        private Dictionary<string, string> _data = new Dictionary<string, string>();

        public string GetLogSource()
        {
            return Get(ToastLoggerFields.LOG_SOURCE);
        }

        public void SetUserId(string userId)
        {
            Put(ToastLoggerFields.USER_ID, userId);
        }

        public string GetUserId()
        {
            return Get(ToastLoggerFields.USER_ID);
        }

        public string GetLogVersion()
        {
            return Get(ToastLoggerFields.LOG_VERSION);
        }

        public ToastLogLevel GetLogLevel()
        {
            return (ToastLogLevel)Enum.Parse(typeof(ToastLogLevel), Get(ToastLoggerFields.LOG_LEVEL));
        }

        public string GetLoggerType()
        {
            return Get(ToastLoggerFields.LOG_TYPE);
        }

        public string GetLogMessage()
        {
            return Get(ToastLoggerFields.LOG_MESSAGE);
        }

        public long GetCreateTime()
        {
            return long.Parse(Get(ToastLoggerFields.LOG_CREATE_TIME));
        }

        public string GetTransactionId()
        {
            return Get(ToastLoggerFields.LOG_TRANSACTION_ID);
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

        public void PutAll(Dictionary<string, string> data)
        {
            var enumerator = data.GetEnumerator();
            while(enumerator.MoveNext())
            {
                _data.Add(enumerator.Current.Key, enumerator.Current.Value); 
            }
        }

        public void SetUserField(string field, string value)
        {
            string convertedField = ToastLoggerFields.ConvertField(field);
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

        public void SetLogSource(string logSource)
        {
            _data.Remove(ToastLoggerFields.LOG_SOURCE);
            Put(ToastLoggerFields.LOG_SOURCE, logSource);
        }

        public string GetProjectKey()
        {
            return Get(ToastLoggerFields.PROJECT_KEY);
        }

        public void SetLogObject(string projectKey, string logType, ToastLogLevel logLevel, string logMessage, string transactionId = "")
        {
            _data.Add(ToastLoggerFields.PROJECT_KEY, projectKey);
            _data.Add(ToastLoggerFields.LOG_TYPE, logType);
            _data.Add(ToastLoggerFields.LOG_SOURCE, ToastLoggerContants.DEFAULT_LOG_SOURCE);
            _data.Add(ToastLoggerFields.LOG_VERSION, ToastLoggerVersion.VERSION);
            _data.Add(ToastLoggerFields.LOG_LEVEL, logLevel.ToString());
            _data.Add(ToastLoggerFields.LOG_MESSAGE, logMessage);
            _data.Add(ToastLoggerFields.LOG_CREATE_TIME, ToastUtil.GetEpochMilliSeconds().ToString());
            _data.Add(ToastLoggerFields.LOG_TRANSACTION_ID, (transactionId == "") ? Guid.NewGuid().ToString() : transactionId);
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

        public string GetDuplicateJSONString()
        {
            JSONNode node = new JSONObject();
            foreach (var parameter in _data)
            {
                if (parameter.Key != ToastLoggerFields.LOG_CREATE_TIME && parameter.Key != ToastLoggerFields.LOG_TRANSACTION_ID)
                {
                    node.Add(parameter.Key, parameter.Value);
                }
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