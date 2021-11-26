using System.Collections.Generic;

namespace Toast.Internal
{
    public class ToastNativeMessage
    {
        private readonly string _uri;
        private readonly string _transactionId;
        private readonly ToastResult _result;
        private readonly Dictionary<string, JSONNode> _extras;

        private ToastNativeMessage(string uri, string transactionId, bool isSuccess, int code, string message)
        {
            _uri = uri;
            _transactionId = transactionId;
            _extras = new Dictionary<string, JSONNode>();
            _result = new ToastResult(isSuccess, code, message);
        }

        public static ToastNativeMessage CreateErrorMessage(string uri, string transactionId, bool isSuccess, int code, string message)
        {
            return CreateMessage(uri,
                                 transactionId,
                                 isSuccess,
                                 code,
                                 message);
        }

        public static ToastNativeMessage CreateSuccessMessage(string uri, string transactionId)
        {
            return CreateMessage(uri,
                                 transactionId,
                                 true,
                                 0,
                                 "Success");
        }

        public static ToastNativeMessage CreateMessage(string uri, string transactionId, bool isSuccess, int code, string message)
        {
            var native = new ToastNativeMessage(uri,
                                                transactionId,
                                                isSuccess,
                                                code,
                                                message);
            return native;
        }

        public ToastNativeMessage AddExtraData(string key, JSONNode value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return this;
            }

            _extras.Add(key, value);
            return this;
        }

        public string ToJsonString()
        {
            JSONNode root = new JSONObject();
            JSONNode header = new JSONObject();
            JSONNode body = new JSONObject();

            body.Add(JsonKeys.IsSuccessful, _result.IsSuccessful);
            body.Add(JsonKeys.ResultCode, _result.Code);
            body.Add(JsonKeys.ResultMessage, _result.Message);

            foreach (string key in _extras.Keys)
            {
                body.Add(key, _extras[key]);
            }

            header.Add(JsonKeys.TransactionId, _transactionId);
            root.Add(JsonKeys.Uri, _uri);
            root.Add(JsonKeys.Header, header);
            root.Add(JsonKeys.Body, body);
            return root.ToString();
        }
    }
}
