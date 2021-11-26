using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Toast.Internal;

namespace Toast.Iap
{
    public static class ToastIap
    {
        private const string SERVICE_NAME = "iap";
#if UNITY_IOS
        private static bool _isRegisterUserIdChangedEvent = false;
#endif
        private static bool _isInitialize = false;
        private static StoreCode _storeCode;

        public delegate void PurchaseUpdateListener(string transactionId, ToastResult result, IapPurchase purchase);

        public static bool Initialize(ToastIapConfiguration configuration, PurchaseUpdateListener listener)
        {
            if (_isInitialize)
            {
                ToastLog.Warn("Already initialize " + typeof(ToastIap).Name);
                return false;
            }

            if (string.IsNullOrEmpty(configuration.AppKey))
            {
                ToastLog.Error("AppKey MUST not be null or empty string");
                return false;
            }

#if UNITY_IOS
            /*
             * Android, iOS 초기화 Flow가 다르므로 iOS는 사용자 ID가 없어도 초기화 호출이 가능하도록 함  
             * 사용자 ID가 없을 경우, 사용자 ID 변경에 대한 이벤트를 등록한다. 사용자 ID 설정시 IAP 초기화가 다시 발생하도록 함
             */
            if (string.IsNullOrEmpty(ToastSdk.UserId))
            {
                if (!_isRegisterUserIdChangedEvent)
                {
                    ToastSdk.UserIdChanged += userId => Initialize(configuration, listener);
                    _isRegisterUserIdChangedEvent = true;
                }

                return true;
            }
#endif

            PurchaseUpdateReceiver.AttachReceiverIfNothing();
            PurchaseUpdateReceiver.SetLoggerListener(listener);

            var methodName = MethodBase.GetCurrentMethod().Name;
            var uri = ToastUri.Create(SERVICE_NAME, methodName.ToLower());
            var methodCall = MethodCall.CreateSyncCall(uri)
                .AddParameter("appKey", configuration.AppKey)
                .AddParameter("serviceZone", configuration.ServiceZone.ToString().ToUpper())
                .AddParameter("updatePurchase", "OnUpdatePurchase");

            string storeCode;
            _storeCode = configuration.StoreCode;
            switch (configuration.StoreCode)
            {
                case StoreCode.GooglePlayStore:
                    storeCode = "GG";
                    break;
                case StoreCode.AppleAppStore:
                    storeCode = "AS";
                    break;
                case StoreCode.OneStore:
                    storeCode = "ONESTORE";
                    break;
                case StoreCode.Redbean:
                    storeCode = "REDBEAN";
                    break;
                case StoreCode.GalaxyStore:
                    storeCode = "GALAXY";
                    break;
                default:
                    ToastLog.Error("Unsupported store code : {0}", configuration.StoreCode);
                    return false;
            }

            methodCall.AddParameter("storeCode", storeCode);

            var response = ToastNativeSender.SyncSendMessage(methodCall);
            if (response != null && response.Result.IsSuccessful)
            {
                _isInitialize = true;

                return true;
            }

            return false;
        }

        public static void RequestProductDetails(ToastCallback<ProductDetailsResult> callback)
        {
            var uri = ToastUri.Create(SERVICE_NAME, "products");
            var methodCall = MethodCall.CreateAsyncCallWithCallback(uri);

            if (callback != null)
            {
                ToastCallbackManager.Instance.AddCallback(methodCall.TransactionId,
                    ConvertIAPErrorCallback(callback),
                    response =>
                    {
                        if (!response.Result.IsSuccessful) return null;

                        var productsJsonArray = response.Body["products"].AsArray;
                        var invalidProductsJsonArray = response.Body["invalidProducts"].AsArray;

                        return new ProductDetailsResult
                        {
                            Products = productsJsonArray.Children
                                .Select(product => product.AsObject)
                                .Select(IapProduct.From)
                                .ToList(),
                            InvalidProducts = invalidProductsJsonArray.Children
                                .Select(product => product.AsObject)
                                .Select(IapProduct.From)
                                .ToList()
                        };
                    });
            }

            ToastNativeSender.SendMessage(methodCall);
        }

        public static string Purchase(string productId, string developerPayload = "")
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var uri = ToastUri.Create(SERVICE_NAME, methodName.ToLower());
            var methodCall = MethodCall.CreateSyncCall(uri)
                .AddParameter("productId", productId);
            if (string.IsNullOrEmpty(developerPayload) == false)
            {
                methodCall.AddParameter("developerPayload", developerPayload);
            }
            ToastNativeSender.SyncSendMessage(methodCall);
            PurchaseUpdateReceiver.SetTransactionId(methodCall.TransactionId);
            return methodCall.TransactionId;
        }

        public static void RequestConsumablePurchases(ToastCallback<List<IapPurchase>> callback)
        {
            var uri = ToastUri.Create(SERVICE_NAME, "purchases/consumable");

            var methodCall = MethodCall.CreateAsyncCallWithCallback(uri);
            if (callback != null)
            {
                ToastCallbackManager.Instance.AddCallback(methodCall.TransactionId,
                    ConvertIAPErrorCallback(callback),
                    ConvertIapPurchases);
            }

            ToastNativeSender.SendMessage(methodCall);
        }

        [Obsolete("Please use RequestActivatedPurchases() instead of RequestActivePurchase()")]
        public static void RequestActivePurchase(ToastCallback<List<IapPurchase>> callback)
        {
            RequestActivatedPurchases(callback);
        }

        public static void RequestActivatedPurchases(ToastCallback<List<IapPurchase>> callback)
        {
            var uri = ToastUri.Create(SERVICE_NAME, "purchases/activated");

            var methodCall = MethodCall.CreateAsyncCallWithCallback(uri);
            if (callback != null)
            {
                ToastCallbackManager.Instance.AddCallback(methodCall.TransactionId,
                    ConvertIAPErrorCallback(callback),
                    ConvertIapPurchases);
            }

            ToastNativeSender.SendMessage(methodCall);
        }

        public static void RequestRestorePurchases(ToastCallback<List<IapPurchase>> callback)
        {
            var uri = ToastUri.Create(SERVICE_NAME, "purchases/restore");

            var methodCall = MethodCall.CreateAsyncCallWithCallback(uri);
            if (callback != null)
            {
                ToastCallbackManager.Instance.AddCallback(methodCall.TransactionId,
                    ConvertIAPErrorCallback(callback),
                    ConvertIapPurchases);
            }

            ToastNativeSender.SendMessage(methodCall);
        }

        public static void RequestSubscriptionsStatus(ToastCallback<List<IapSubscriptionStatus>> callback)
        {
            RequestSubscriptionsStatus(false, callback);
        }

        public static void RequestSubscriptionsStatus(
            bool includeExpiredSubscriptions,
            ToastCallback<List<IapSubscriptionStatus>> callback)
        {
            var uri = ToastUri.Create(SERVICE_NAME, "subscriptions/status");
            var methodCall = MethodCall.CreateAsyncCallWithCallback(uri);
            methodCall.AddParameter("includeExpiredSubscriptions", includeExpiredSubscriptions);
            if (callback != null)
            {
                ToastCallbackManager.Instance.AddCallback(methodCall.TransactionId,
                    ConvertIAPErrorCallback(callback),
                    ConvertIapSubscriptionsStatus);
            }
            ToastNativeSender.SendMessage(methodCall);
        }

        private static ToastCallback<T> ConvertIAPErrorCallback<T>(ToastCallback<T> callback)
        {
            return (result, value) =>
            {
                var toastIapError = new ToastIapError(result.Code);
                var convertResult = new ToastResult(
                    result.IsSuccessful,
                    toastIapError.Code,
                    result.Message);

                callback(convertResult, value);
            };
        }

        private static List<IapPurchase> ConvertIapPurchases(NativeResponse response)
        {
            if (!response.Result.IsSuccessful) return null;

            var body = response.Body;
            if (!body.ContainsKey("purchases"))
            {
                return new List<IapPurchase>();
            }

            var purchases = body["purchases"].AsArray;
            return purchases.Children.Select(purchase => IapPurchase.From(purchase.AsObject)).ToList();
        }

        private static List<IapSubscriptionStatus> ConvertIapSubscriptionsStatus(NativeResponse response)
        {
            if (!response.Result.IsSuccessful) return null;

            var body = response.Body;
            if (!body.ContainsKey("purchases"))
            {
                return new List<IapSubscriptionStatus>();
            }

            var purchases = body["purchases"].AsArray;
            return purchases.Children.Select(purchase => IapSubscriptionStatus.From(purchase.AsObject)).ToList();
        }

        public static bool IsInitialize()
        {
            return _isInitialize;
        }

        public static StoreCode GetStoreCode()
        {
            return _storeCode;
        }
    }
}