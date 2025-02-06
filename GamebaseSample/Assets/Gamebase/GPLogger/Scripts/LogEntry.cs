using GamePlatform.Logger.Internal;
using GamePlatform.Logger.ThirdParty;
using System;
using System.Collections.Generic;

namespace GamePlatform.Logger
{
    public class LogEntry
    {
        public string LogType { get; set; }
        public GpLogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
        public long CreateTime { get; set; }
        public Dictionary<string, string> UserFields { get; set; }

        public LogEntry()
        {
            UserFields = new Dictionary<string, string>();
        }

        public static LogEntry From(GpLoggerResponse.Common.Log log)
        {
            var result = new LogEntry
            {
                LogType = log.type,
                LogLevel = (GpLogLevel)Enum.Parse(typeof(GpLogLevel), log.level),
                Message = log.message,
                TransactionId = log.transactionId,
                CreateTime = log.createTime,
                UserFields = log.userFields
            };

            return result;
        }

        public void ClearUserFields()
        {
            UserFields.Clear();
        }

        public override string ToString()
        {
            return JsonMapper.ToJson(this);
        }
    }
}