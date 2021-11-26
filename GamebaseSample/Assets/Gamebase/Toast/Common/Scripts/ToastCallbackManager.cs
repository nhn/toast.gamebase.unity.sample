using System;
using System.Collections.Generic;
using System.Linq;
using Toast.Internal;

namespace Toast
{
    public class ToastCallbackManager
    {
        public delegate void ToastRegisterCallback(ToastResult result, NativeResponse response);

        private readonly Dictionary<string, ToastRegisterCallback> _callbackTable;

        static ToastCallbackManager()
        {
            Instance = new ToastCallbackManager();
        }

        private ToastCallbackManager()
        {
            _callbackTable = new Dictionary<string, ToastRegisterCallback>();
        }

        public static ToastCallbackManager Instance { get; private set; }

        internal IList<string> RegisteredCallbackIds
        {
            get { return _callbackTable.Select(kv => kv.Key).ToList(); }
        }

        public ToastRegisterCallback this[string id]
        {
            get
            {
                if (!_callbackTable.ContainsKey(id))
                {
                    return null;
                }

                return _callbackTable[id];
            }
        }

        public void AddCallback<TResult>(
            string transactionId,
            ToastCallback<TResult> callback,
            Converter<NativeResponse, TResult> converter)
        {
            AddCallbackInternal(transactionId, callback, converter);
        }

        private void AddCallbackInternal<TResult>(
            string transactionId,
            ToastCallback<TResult> callback,
            Converter<NativeResponse, TResult> converter)
        {
            ToastRegisterCallback toastRegisterCallback = (result, json) => callback(result, converter(json));
            _callbackTable.Add(transactionId, toastRegisterCallback);
        }

        public void RemoveCallback(string callbackId)
        {
            if (_callbackTable.ContainsKey(callbackId))
            {
                _callbackTable.Remove(callbackId);
            }
        }
    }
}