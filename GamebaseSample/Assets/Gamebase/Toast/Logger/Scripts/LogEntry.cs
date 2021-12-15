using System;
using System.Collections.Generic;
using Toast.Internal;

namespace Toast.Logger
{
    public class LogEntry
    {
        private class Keys
        {
            internal const string Type = "type";
            internal const string Level = "level";
            internal const string Message = "message";
            internal const string TransactionId = "transactionId";
            internal const string CreateTime = "createTime";
            internal const string UserFields = "userFields";
        }

        public string LogType { get; set; }
        public ToastLogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
        public long CreateTime { get; set; }
        public Dictionary<string, object> UserFields { get; set; }

        public LogEntry()
        {
            UserFields = new Dictionary<string, object>();
        }

        internal static LogEntry From(JSONObject logJson)
        {
            var result = new LogEntry
            {
                LogType = logJson[Keys.Type],
                LogLevel = (ToastLogLevel)Enum.Parse(typeof(ToastLogLevel), logJson[Keys.Level]),
                Message = logJson[Keys.Message],
                TransactionId = logJson[Keys.TransactionId],
                CreateTime = logJson[Keys.CreateTime]
            };

            const string userFieldsKey = Keys.UserFields;
            if (logJson.ContainsKey(userFieldsKey))
            {
                if (logJson[userFieldsKey].IsObject)
                {
                    var userFieldsJsonObject = logJson[userFieldsKey].AsObject;
                    result.UserFields = userFieldsJsonObject.ToDictionary();
                }
            }

            return result;
        }

        internal void ClearUserFields()
        {
            UserFields.Clear();
        }

        public override string ToString()
        {
            var json = new JSONObject();
            json.Add(Keys.Type, LogType);
            json.Add(Keys.Level, LogLevel.ToString());
            json.Add(Keys.Message, Message);
            json.Add(Keys.TransactionId, TransactionId);
            json.Add(Keys.CreateTime, CreateTime);

            if (UserFields.Count > 0)
            {
                var userFields = JSONObject.FromDictionary(UserFields);
                json.Add(Keys.UserFields, userFields);
            }

            return json.ToString(4);
        }
    }
}