using System.Collections.Generic;
using UnityEngine;

using Toast.Internal;

#if USE_ONGATE
using Toast.Iap.Ongate;
#endif // USE_ONGATE

namespace Toast.Iap
{
    public static class ToastGamebaseIap
    {
        public const string EXTRA_APP_ID = "AppID";
        public const string EXTRA_USER_ID = "UserID";

        private static void ErrorCallback<T>(ToastCallback<T> callback, int errorCode, string errorMessage) where T : class
        {
            if (callback != null)
            {
                ToastResult result = new ToastResult(false, errorCode, errorMessage);
                callback(result, null);
            }
        }

        public static void Initialize(ToastGamebaseIapConfiguration configuration, ToastIap.PurchaseUpdateListener listener)
        {
#if USE_ONGATE
            var extraData = configuration.ExtraData;

            if (extraData == null)
            {
                return;
            }

            if (extraData.ContainsKey(ToastGamebaseIapConfiguration.STORE_ONGATE))
            {
                Dictionary<string, string> ongateExtra = extraData[ToastGamebaseIapConfiguration.STORE_ONGATE];
                var serviceZone = GetServerPhase(configuration.ServiceZone);

                // EXTRA_APP_ID가 포함되어 있다면
                if (ongateExtra.ContainsKey(EXTRA_APP_ID))
                {
                    var appId = ongateExtra[EXTRA_APP_ID];

                    bool isAppKeyNum = IsAppKeyNum(appId);
                    if (isAppKeyNum == false)
                    {
                        Debug.LogError("Ongate's appId should only be a number.");
                        return;
                    }

                    ToastIapOngate.SetAppId(System.Convert.ToInt64(appId));
                    ToastIapOngate.SetServiceZone(serviceZone);
                    ToastIapOngate.Initialize();
                }

                // userId가 포함되어 있다면
                if (ongateExtra.ContainsKey(EXTRA_USER_ID))
                {
                    var userId = ongateExtra[EXTRA_USER_ID];
                    ToastIapOngate.SetOngateUserId(userId);
                }
            }
            else
            {
                ToastLog.Error(ToastGamebaseIapErrorCode.IncollectStoreCode.Message);
            }
#else
                ToastLog.Error(ToastGamebaseIapErrorCode.OngateNotDefined.Message);
#endif // USE_ONGATE
        }
      
        public static void SetExtraData(string storeCode, Dictionary<string, string> storeExtras)
        {
            if (storeExtras == null)
            {
                return;
            }

#if USE_ONGATE
            if (storeCode == ToastGamebaseIapConfiguration.STORE_ONGATE)
            {
                if (storeExtras.ContainsKey(EXTRA_USER_ID))
                {
                    ToastIapOngate.SetOngateUserId(storeExtras[EXTRA_USER_ID]);
                }
            }
            else
            {
                ToastLog.Error(ToastGamebaseIapErrorCode.IncollectStoreCode.Message);
            }
#else
            ToastLog.Error(ToastGamebaseIapErrorCode.OngateNotDefined.Message);
#endif // USE_ONGATE
        }

        public static void RequestProductDetails(string storeCode, ToastCallback<ProductDetailsResult> callback)
        {

#if USE_ONGATE
            if (storeCode == ToastGamebaseIapConfiguration.STORE_ONGATE)
            {
                ToastIapOngate.RequestProductDetails((Result result, object data) =>
                {
                    ToastResult toastResult = new ToastResult(result.isSuccessful, result.resultCode, result.resultString);

                    if (result.IsSuccessful == false)
                    {
                        Debug.Log("RequestProductDetails.OnCallback() -> Failed! -> " + result.ResultCode + ":" + result.ResultString);
                        if (callback != null)
                        {
                            callback(toastResult, null);
                        }
                        return;
                    }

                    if (callback != null)
                    {
                        string jsonString = System.Convert.ToString(data);

                        ProductDetailsResult productDetailsResult = new ProductDetailsResult();

                        JSONArray jsonArray = JSONNode.Parse(jsonString).AsArray;
                        if (jsonArray.Count > 0)
                        {
                            productDetailsResult.Products = new List<IapProduct>();
                            foreach (JSONNode item in jsonArray)
                            {
                                IapProduct iapProduct = new IapProduct();
                                iapProduct.ProductSeq = item["itemSeq"];
#pragma warning disable 0612
                                iapProduct.ProductName = item["itemName"];
#pragma warning restore 0612
                                iapProduct.ProductId = item["marketItemId"]; ;
                                iapProduct.Currency = item["currency"];
                                iapProduct.Price = item["price"];
                                productDetailsResult.Products.Add(iapProduct);
                            }
                        }
                        callback(toastResult, productDetailsResult);
                    }
                });
            }
            else
            {
                ErrorCallback<ProductDetailsResult>(callback, ToastGamebaseIapErrorCode.IncollectStoreCode.Code, ToastGamebaseIapErrorCode.IncollectStoreCode.Message);
            }
#else
            ToastLog.Error(ToastGamebaseIapErrorCode.OngateNotDefined.Message);
            ErrorCallback<ProductDetailsResult>(callback, ToastGamebaseIapErrorCode.OngateNotDefined.Code, ToastGamebaseIapErrorCode.OngateNotDefined.Message);
#endif // USE_ONGATE
        }

        public static void Purchase(string storeCode, string productId, ToastCallback<IapPurchase> callback)
        {
            Purchase(storeCode, productId, "", callback);
        }

        public static void Purchase(string storeCode, string productId, string developerPayload, ToastCallback<IapPurchase> callback)
        {
#if USE_ONGATE
            if (storeCode == ToastGamebaseIapConfiguration.STORE_ONGATE)
            {
                ToastIapOngate.Purchase(productId, (Result result, object data) =>
                {
                    ToastResult toastResult = new ToastResult(result.isSuccessful, result.resultCode, result.resultString);

                    if (result.IsSuccessful == false)
                    {
                        Debug.Log("Purchase.OnCallback() -> Failed! -> " + result.ResultCode + ":" + result.ResultString);
                        if (callback != null)
                        {
                            callback(toastResult, null);
                        }
                        return;
                    }

                    if (callback != null)
                    {
                        string jsonString = System.Convert.ToString(data);

                        var jsonNode = JSONNode.Parse(jsonString);

                        IapPurchase iapPurchase = new IapPurchase();
                        iapPurchase.PaymentSequence = jsonNode["paymentSeq"];
                        iapPurchase.ItemSeq = jsonNode["itemSeq"];
                        iapPurchase.UserId = ToastCoreSdk.Instance.NativeCore.GetUserId();
                        iapPurchase.PriceCurrencyCode = jsonNode["currency"];
                        iapPurchase.Price = jsonNode["price"];
                        iapPurchase.AccessToken = jsonNode["purchaseToken"];
                        callback(toastResult, iapPurchase);
                    }
                });
            }
            else
            {
                ErrorCallback<IapPurchase>(callback, ToastGamebaseIapErrorCode.IncollectStoreCode.Code, ToastGamebaseIapErrorCode.IncollectStoreCode.Message);
            }
#else
            ToastLog.Error(ToastGamebaseIapErrorCode.OngateNotDefined.Message);
            ErrorCallback<IapPurchase>(callback, ToastGamebaseIapErrorCode.OngateNotDefined.Code, ToastGamebaseIapErrorCode.OngateNotDefined.Message);
#endif  // USE_ONGATE
        }

        public static void RequestConsumablePurchases(string storeCode, ToastCallback<List<IapPurchase>> callback)
        {
#if USE_ONGATE
            if (storeCode == ToastGamebaseIapConfiguration.STORE_ONGATE)
            {
                ToastIapOngate.RequestConsumablePurchases((Result result, object data) =>
                {
                    ToastResult toastResult = new ToastResult(result.isSuccessful, result.resultCode, result.resultString);

                    if (result.IsSuccessful == false)
                    {
                        Debug.Log("RequestConsumablePurchases.OnCallback() -> Failed! -> " + result.ResultCode + ":" + result.ResultString);
                        if (callback != null)
                        {
                            callback(toastResult, null);
                        }
                        return;
                    }

                    if (callback != null)
                    {
                        List<IapPurchase> value = new List<IapPurchase>();

                        string jsonString = System.Convert.ToString(data);

                        JSONArray jsonArray = JSONNode.Parse(jsonString).AsArray;
                        foreach (JSONNode item in jsonArray)
                        {
                            IapPurchase iapPurchase = new IapPurchase();
                            iapPurchase.PaymentSequence = item["paymentSeq"];
                            iapPurchase.ProductId = item["itemSeq"].ToString();
                            iapPurchase.UserId = ToastCoreSdk.Instance.NativeCore.GetUserId();
                            iapPurchase.PriceCurrencyCode = item["currency"];
                            iapPurchase.Price = item["price"];
                            iapPurchase.AccessToken = item["purchaseToken"];
                            value.Add(iapPurchase);
                        }
                        callback(toastResult, value);
                    }
                });
            }
            else
            {
                ErrorCallback<List<IapPurchase>>(callback, ToastGamebaseIapErrorCode.IncollectStoreCode.Code, ToastGamebaseIapErrorCode.IncollectStoreCode.Message);
            }
#else
            ToastLog.Error("To use Ongate, you must define USE_ONGATE.");
            ErrorCallback<List<IapPurchase>>(callback, ToastGamebaseIapErrorCode.OngateNotDefined.Code, ToastGamebaseIapErrorCode.OngateNotDefined.Message);
#endif  // UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        }

        public static void RequestActivatedPurchases(string storeCode, ToastCallback<List<IapPurchase>> callback)
        {

#if USE_ONGATE
            if (storeCode == ToastGamebaseIapConfiguration.STORE_ONGATE)
            {
                ErrorCallback<List<IapPurchase>>(callback, ToastGamebaseIapErrorCode.NotSupportedMethod.Code, ToastGamebaseIapErrorCode.NotSupportedMethod.Message);
            }
            else
            {
                ErrorCallback<List<IapPurchase>>(callback, ToastGamebaseIapErrorCode.IncollectStoreCode.Code, ToastGamebaseIapErrorCode.IncollectStoreCode.Message);
            }
#else
             ToastLog.Error(ToastGamebaseIapErrorCode.OngateNotDefined.Message);
                ErrorCallback<List<IapPurchase>>(callback, ToastGamebaseIapErrorCode.OngateNotDefined.Code, ToastGamebaseIapErrorCode.OngateNotDefined.Message);
#endif  // USE_ONGATE
        }

        public static void RequestRestorePurchases(string storeCode, ToastCallback<List<IapPurchase>> callback)
        {

#if USE_ONGATE
            if (storeCode == ToastGamebaseIapConfiguration.STORE_ONGATE)
            {
                ErrorCallback<List<IapPurchase>>(callback, ToastGamebaseIapErrorCode.NotSupportedMethod.Code, ToastGamebaseIapErrorCode.NotSupportedMethod.Message);
            }
            else
            {
                ErrorCallback<List<IapPurchase>>(callback, ToastGamebaseIapErrorCode.IncollectStoreCode.Code, ToastGamebaseIapErrorCode.IncollectStoreCode.Message);
            }
#else
            ToastLog.Error(ToastGamebaseIapErrorCode.OngateNotDefined.Message);
            ErrorCallback<List<IapPurchase>>(callback, ToastGamebaseIapErrorCode.OngateNotDefined.Code, ToastGamebaseIapErrorCode.OngateNotDefined.Message);
#endif  // USE_ONGATE
        }

#if USE_ONGATE
        public static void SetOngateDebugMode(bool debugMode)
        {
            ToastIapOngate.SetDebugMode(debugMode);
        }

        private static ServerPhase GetServerPhase(ToastServiceZone serviceZone)
        {
            ServerPhase zone = ServerPhase.ALPHA;
            if (serviceZone == ToastServiceZone.BETA)
            {
                zone = ServerPhase.BETA;
            }
            else if (serviceZone == ToastServiceZone.REAL)
            {
                zone = ServerPhase.REAL;
            }

            return zone;
        }
#endif  // USE_ONGATE

        private static bool IsAppKeyNum(string appKey)
        {
            long num;
            bool isAppKeyNum = long.TryParse(appKey, out num);
            return isAppKeyNum;
        }
    }
}
