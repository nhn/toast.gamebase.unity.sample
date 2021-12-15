using System;
using System.Collections.Generic;

namespace Toast.Internal
{
    public class CallbackInfo
    {
        public string ObjectName { get; set; }
        public string MethodName { get; set; }
    }

    public class MethodCall
    {
        private readonly string _url;
        private readonly Dictionary<string, JSONNode> _parameters;

        private string _receiveObject;
        private string _receiveMethod;

        private readonly string _transactionId;

        public string TransactionId
        {
            get { return _transactionId; }
        }

        private MethodCall(string url)
        {
            _transactionId = Guid.NewGuid().ToString();
            _url = url;
            _parameters = new Dictionary<string, JSONNode>();
        }

        public static MethodCall CreateSyncCall(string url)
        {
            return new MethodCall(url);
        }

        public static MethodCall CreateAsyncCallWithCallback(string url)
        {
            return CreateAsyncCallWithCallback(url, Constants.SdkPluginObjectName,
                Constants.SdkPluginReceiveMethodName);
        }

        public static MethodCall CreateAsyncCallWithCallback(string url, string returnObject,
            string returnMethod)
        {
            var ret = new MethodCall(url);
            ret.SetCallback(returnObject, returnMethod);
            return ret;
        }

        private void SetCallback(string receiveObject, string receiveMethod)
        {
            if (string.IsNullOrEmpty(receiveObject) || string.IsNullOrEmpty(receiveMethod))
            {
                return;
            }

            _receiveObject = receiveObject;
            _receiveMethod = receiveMethod;
        }

        public MethodCall AddParameter(string key, JSONNode value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return this;
            }

            _parameters.Add(key, value);
            return this;
        }

        public MethodCall AddParameter(string key, IDictionary<string, string> value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return this;
            }

            var param = new JSONObject();
            foreach (var kv in value)
            {
                string strKey = kv.Key;
                string strValue = kv.Value;
                if (string.IsNullOrEmpty(strKey))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(strValue))
                {
                    strValue = "";
                }

                param.Add(strKey, strValue);
            }

            _parameters.Add(key, param);
            return this;
        }

        public string ToJsonString()
        {
            JSONNode header = new JSONObject();
            JSONNode root = new JSONObject();

            header.Add(JsonKeys.TransactionId, _transactionId);
            if (!string.IsNullOrEmpty(_transactionId)
                && !string.IsNullOrEmpty(_receiveObject)
                && !string.IsNullOrEmpty(_receiveMethod))
            {
                JSONNode callback = new JSONObject();
                callback.Add(JsonKeys.ReceiveObjectName, _receiveObject);
                callback.Add(JsonKeys.ReceiveMethodName, _receiveMethod);
                header.Add(JsonKeys.Callback, callback);
            }

            if (_parameters != null && _parameters.Count > 0)
            {
                JSONNode payload = new JSONObject();
                foreach (var parameter in _parameters)
                {
                    payload.Add(parameter.Key, parameter.Value);
                }

                root.Add(JsonKeys.Payload, payload);
            }

            root.Add(JsonKeys.Uri, _url);
            root.Add(JsonKeys.Header, header);
            return root.ToString();
        }

        public override string ToString()
        {
            return string.Format("TransactionId: {0}, Uri : {1}", TransactionId, _url);
        }
    }
}