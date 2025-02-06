using GamePlatform.Logger.ThirdParty;
using System;
using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public class LoggerProtocol
    {
        public readonly string uri;
        public readonly Dictionary<string, object> header;
        public readonly Dictionary<string, object> payload;

        public LoggerProtocol(string apiScheme)
        {
            uri = apiScheme;

            header = new Dictionary<string, object>
            {
                { "transactionId", Guid.NewGuid().ToString() },
            };

            payload = new Dictionary<string, object>();
        }

        public void AddParameter(string key, object value)
        {
            if (string.IsNullOrEmpty(key) == true || value == null)
            {
                return;
            }

            payload.Add(key, value);
        }

        public override string ToString()
        {
            return JsonMapper.ToJson(this);
        }
    }
}