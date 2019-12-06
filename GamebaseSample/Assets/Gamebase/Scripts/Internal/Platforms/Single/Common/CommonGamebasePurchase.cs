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
                    GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.PURCHASE,
                            GamebaseIndicatorReportType.StabilityCode.GB_IAP_PURCHASE_SUCCESS,
                            GamebaseIndicatorReportType.LogLevel.INFO,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_TCIAP_APP_KEY, launchingInfo.tcProduct.iap.appKey},
                                { GamebaseIndicatorReportType.AdditionalKey.GB_ITEM_SEQ, itemSeq.ToString() }
                            });
                }
                else
                {
                    if (error.code == GamebaseErrorCode.PURCHASE_USER_CANCELED)
                    {
                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.PURCHASE,
                                GamebaseIndicatorReportType.StabilityCode.GB_IAP_PURCHASE_CANCELED,
                                GamebaseIndicatorReportType.LogLevel.DEBUG,
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
                (receipt, error) =>
                {
                    if (Gamebase.IsSuccess(error) == true)
                    {
                        GamebaseAnalytics.Instance.CompletePurchase(receipt);
                    }
                    else
                    {
                    }
                    puechaseCallback(receipt, error);
                });
        }

        public void RequestPurchase(string marketItemId, int handle)
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

            PurchaseAdapterManager.Instance.RequestItemListOfNotConsumed(callback);
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
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.IapPurchase>>>(handle));
        }

        private bool IsLoggedIn()
        {
            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
            {
                return false;
            }

            return true;
        }       
    }
}
#endif