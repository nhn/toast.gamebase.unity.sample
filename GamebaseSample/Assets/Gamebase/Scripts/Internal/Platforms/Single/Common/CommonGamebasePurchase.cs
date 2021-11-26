#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebasePurchase : IGamebasePurchase
    {
        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain) == true)
                {
                    return typeof(CommonGamebasePurchase).Name;
                }

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public CommonGamebasePurchase()
        {
                    
        }

        public void RequestPurchase(long itemSeq, int handle)
        {
            GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt> puechaseCallback = (purchasableReceipt, error) =>
            {
                GamebaseResponse.Launching.LaunchingInfo launchingInfo = GamebaseLaunchingImplementation.Instance.GetLaunchingInformations();
                if (Gamebase.IsSuccess(error) == true)
                {
                    Dictionary<string, string> custom = new Dictionary<string, string>()
                    {
                        { GamebaseIndicatorReportType.AdditionalKey.GB_TCIAP_APP_KEY, launchingInfo.tcProduct.iap.appKey},
                        { GamebaseIndicatorReportType.AdditionalKey.GB_ITEM_SEQ, itemSeq.ToString() }
                    };

                    if (GamebaseIndicatorReport.GetLogLevel() <= GamebaseIndicatorReport.LogLevel.DEBUG)
                    {
                        custom.Add(GamebaseIndicatorReportType.AdditionalKey.GB_TAA_USER_LEVEL, GamebaseAnalytics.Instance.UserLevel.ToString());
                        custom.Add(GamebaseIndicatorReportType.AdditionalKey.GB_PAYMENT_SEQ, purchasableReceipt.paymentSeq);
                    }

                    GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.PURCHASE,
                            GamebaseIndicatorReportType.StabilityCode.GB_IAP_PURCHASE_SUCCESS,
                            GamebaseIndicatorReportType.LogLevel.INFO,
                            custom);
                }
                else
                {
                    if (error.code == GamebaseErrorCode.PURCHASE_USER_CANCELED)
                    {
                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.PURCHASE,
                                GamebaseIndicatorReportType.StabilityCode.GB_IAP_PURCHASE_CANCELED,
                                GamebaseIndicatorReportType.LogLevel.INFO,
                                new Dictionary<string, string>()
                                {
                                    { GamebaseIndicatorReportType.AdditionalKey.GB_TCIAP_APP_KEY, launchingInfo.tcProduct.iap.appKey},
                                    { GamebaseIndicatorReportType.AdditionalKey.GB_ITEM_SEQ, itemSeq.ToString() }
                                },
                                error,
                                true);
                    }
                    else
                    {
                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.PURCHASE,
                                GamebaseIndicatorReportType.StabilityCode.GB_IAP_PURCHASE_FAILED,
                                GamebaseIndicatorReportType.LogLevel.DEBUG,
                                new Dictionary<string, string>()
                                {
                                    { GamebaseIndicatorReportType.AdditionalKey.GB_TCIAP_APP_KEY, launchingInfo.tcProduct.iap.appKey},
                                    { GamebaseIndicatorReportType.AdditionalKey.GB_ITEM_SEQ, itemSeq.ToString() }
                                },
                                error,
                                true);
                    }
                }

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt>>(handle);

                if (callback != null)
                {
                    callback(purchasableReceipt, error);
                }

                GamebaseCallbackHandler.UnregisterCallback(handle);
            };
            
            if (IsLoggedIn() == false)
            {
                puechaseCallback(null, new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

            PurchaseAdapterManager.Instance.RequestPurchase(
                itemSeq,
                (purchasableReceipt, error) =>
                {
                    if (Gamebase.IsSuccess(error) == true)
                    {
                        CompletePurchase(purchasableReceipt);
                    }
                    else
                    {
                    }
                    puechaseCallback(purchasableReceipt, error);
                });
        }

        public void RequestPurchase(string gamebaseProductId, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt>>(handle));
        }

        public void RequestPurchase(string gamebaseProductId, string payload, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt>>(handle));
        }

        public void RequestItemListPurchasable(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableItem>>>(handle);

            if (IsLoggedIn() == false)
            {
                callback(null, new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

            PurchaseAdapterManager.Instance.RequestItemListPurchasable(callback);
        }

        public void RequestItemListAtIAPConsole(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableItem>>>(handle);

            if (IsLoggedIn() == false)
            {
                callback(null, new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

            PurchaseAdapterManager.Instance.RequestItemListAtIAPConsole(callback);
        }

        public void RequestItemListOfNotConsumed(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>>>(handle);

            if (IsLoggedIn() == false)
            {
                callback(null, new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

           GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>> responseCallback = (ItemList, error) =>
            {
                callback(ItemList, error);

                if (Gamebase.IsSuccess(error) == true && ItemList != null)
                {
                    foreach(GamebaseResponse.Purchase.PurchasableReceipt purchasableReceipt in ItemList)
                    {
                        CompletePurchase(purchasableReceipt);
                    }
                }
            };

            PurchaseAdapterManager.Instance.RequestItemListOfNotConsumed(responseCallback);
        }

        public void RequestRetryTransaction(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableRetryTransactionResult>>(handle);

            if (IsLoggedIn() == false)
            {
                callback(null, new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

            PurchaseAdapterManager.Instance.RequestRetryTransaction(callback);
        }
        
        public void SetStoreCode(string storeCode)
        {
            GamebaseUnitySDK.StoreCode = storeCode;
        }

        public void SetPromotionIAPHandler(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt>>(handle));
        }

        public string GetStoreCode()
        {
            return GamebaseUnitySDK.StoreCode;
        }

        public void RequestActivatedPurchases(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>>>(handle));
        }

        private bool IsLoggedIn()
        {
            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
            {
                return false;
            }

            return true;
        }

        private void CompletePurchase(GamebaseResponse.Purchase.PurchasableReceipt purchasableReceipt)
        {
            GamebaseAnalytics.Instance.CompletePurchase(purchasableReceipt,
                (analyticsError) =>
                {
                    string stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_PURCHASE_COMPLETE_SUCCESS;
                    if (Gamebase.IsSuccess(analyticsError) == false)
                    {
                        stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_PURCHASE_COMPLETE_FAILED;
                    }

                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.PURCHASE,
                        stabilityCode,
                        GamebaseIndicatorReportType.LogLevel.DEBUG,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_TAA_USER_LEVEL, GamebaseAnalytics.Instance.UserLevel.ToString()},
                            { GamebaseIndicatorReportType.AdditionalKey.GB_STORE_CODE, GamebaseUnitySDK.StoreCode},
                            { GamebaseIndicatorReportType.AdditionalKey.GB_PAYMENT_SEQ, purchasableReceipt.paymentSeq }
                        },
                        analyticsError);
                });
        }
    }
}
#endif