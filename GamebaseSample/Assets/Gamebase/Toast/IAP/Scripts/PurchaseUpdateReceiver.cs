using Toast.Internal;
using UnityEngine;

namespace Toast.Iap
{
    public class PurchaseUpdateReceiver : MonoBehaviour
    {
        private ToastIap.PurchaseUpdateListener _listener;
        private static PurchaseUpdateReceiver _instance;
        private static string _transactionId;

        public static void AttachReceiverIfNothing()
        {
            if (_instance == null)
            {
                var gameObject = ToastNativePlugin.Instance.gameObject;
                var currentReceiver = gameObject.GetComponent<PurchaseUpdateReceiver>();
                if (currentReceiver == null)
                {
                    _instance = gameObject.AddComponent<PurchaseUpdateReceiver>();
                }
            }
        }

        public static void SetLoggerListener(ToastIap.PurchaseUpdateListener listener)
        {
            if (_instance != null)
            {
                _instance._listener = listener;
            }
        }

        public static void SetTransactionId(string transactionId)
        {
            _transactionId = transactionId;
        }

        public static void UpdatePurchase(string jsonString)
        {
            if (_instance != null)
            {
                _instance.OnUpdatePurchase(jsonString);
            }
        }

        public void OnUpdatePurchase(string jsonString)
        {
            Debug.LogFormat("OnUpdatePurchase : {0}", jsonString);
            /*
            {
                "body": {
                    "isSuccessful": false,
                    "resultCode": 50006,
                    "resultMessage": "Purchase is cancelled"
                },
                "uri": "toast://iap/purchase/listener/update",
                "header": {
                    "transactionId": "30713280-4ca2-4238-92ad-50edea1bb0dc"
                }
            }
            */
            var response = NativeResponse.FromJson(jsonString);
            if (response.Result.IsSuccessful)
            {
                var body = response.Body;
                if (body.ContainsKey("purchase"))
                {
                    InvokeListenerSafe(response.Result, IapPurchase.From(body["purchase"].AsObject));
                }
                else
                {
                    InvokeListenerSafe(
                        new ToastResult(false, ToastIapErrorCode.InvalidPurchaseStatus.Code, "Failed to process response : (purchase value is nothing)"),
                        null);
                }
            }
            else
            {
                ToastIapError iapError = new ToastIapError(response.Result.Code);
                ToastResult result = new ToastResult(response.Result.IsSuccessful, iapError.Code, response.Result.Message);
                InvokeListenerSafe(result, null);
            }
        }

        private void InvokeListenerSafe(ToastResult result, IapPurchase purchase)
        {
            if (_listener != null)
            {
                _listener(_transactionId, result, purchase);
            }
        }
    }
}