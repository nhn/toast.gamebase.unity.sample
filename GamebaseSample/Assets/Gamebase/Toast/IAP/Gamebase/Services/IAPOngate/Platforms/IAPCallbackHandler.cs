using System.Collections.Generic;
using Toast.Internal;

namespace Toast.Iap.Ongate
{
    public class IAPCallbackHandler
    {
        public static readonly string TAG = "IAPCallbackHandler";
        public static readonly string EVENT_ID_KEY = "eventId";
        public static readonly string EVENT_KEY = "event";

        private static readonly string RESPONSE_IS_SUCCESS_KEY = "isSuccess";
        private static readonly string RESPONSE_RESULT_KEY = "result";
        private static readonly string RESPONSE_CODE_KEY = "code";
        private static readonly string RESPONSE_MESSAGE_KEY = "message";

        private int eventId = 0;
        private Dictionary<int, OnResponsePurchase> callbacks = new Dictionary<int, OnResponsePurchase>();

        public delegate void OnResponsePurchase(Result result, object data);

        private static class IAPCallbackHandlerHolder
        {
            public static readonly IAPCallbackHandler instance = new IAPCallbackHandler();
        }

        public static IAPCallbackHandler Instance
        {
            get
            {
                return IAPCallbackHandlerHolder.instance;
            }
        }

        public int AddCallback(OnResponsePurchase callback)
        {
            callbacks.Add(eventId, callback);
            return eventId++;
        }

        public OnResponsePurchase GetCallback(int eventId)
        {
            if (callbacks.ContainsKey(eventId))
            {
                return callbacks[eventId];
            }
            return null;
        }

        public void RemoveCallback(int eventId)
        {
            if (callbacks.ContainsKey(eventId))
                callbacks.Remove(eventId);
        }

        public void HandleCallback(string response)
        {
            /**
			 * [response example]
			 * {
			 *   "eventId": 1,
			 *   "event": "REQUEST_PURCHASE",
			 *   "isSuccess": true,
			 *   "code": 0,
			 *   "message": "",
			 *   "result": {}
			 * }
			 **/

            JSONNode root = JSON.Parse(response);
            int eventId = root[EVENT_ID_KEY].AsInt;
            IAPEvent iapEvent = (IAPEvent)System.Enum.Parse(typeof(IAPEvent), root[EVENT_KEY]);
            bool isSuccess = System.Boolean.Parse(root[RESPONSE_IS_SUCCESS_KEY]);
            int code = root[RESPONSE_CODE_KEY].AsInt;
            string message = root[RESPONSE_MESSAGE_KEY];
            var result = root[RESPONSE_RESULT_KEY];

            var callback = GetCallback(eventId);
            if (!isSuccess)
            {
                callback.Invoke(new Result(isSuccess, code, message), null);
            }
            else
            {
                HandleSuccessEvent(iapEvent, result, callback);
            }

            RemoveCallback(eventId);
        }

        private void HandleSuccessEvent(IAPEvent iapEvent, JSONNode root, OnResponsePurchase callback)
        {
            if (callback != null)
                callback.Invoke(new Result(true, ToastIapOngate.API_SUCCESS_CODE, ""), root.ToString());
        }
    }
}
