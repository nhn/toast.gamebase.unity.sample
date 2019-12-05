#if !UNITY_EDITOR && UNITY_ANDROID
using Toast.Gamebase.Internal.Mobile.Android;
#elif !UNITY_EDITOR && UNITY_IOS
using Toast.Gamebase.Internal.Mobile.IOS;
#elif !UNITY_EDITOR && UNITY_WEBGL
using Toast.Gamebase.Internal.Single.WebGL;
#else
using Toast.Gamebase.Internal.Single.Standalone;
#endif
using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public sealed class GamebasePurchaseImplementation
    {
        private static readonly GamebasePurchaseImplementation instance = new GamebasePurchaseImplementation();

        public static GamebasePurchaseImplementation Instance
        {
            get { return instance; }
        }

        IGamebasePurchase purchase;

        private GamebasePurchaseImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            purchase = new AndroidGamebasePurchase();
#elif !UNITY_EDITOR && UNITY_IOS
            purchase = new IOSGamebasePurchase();
#elif !UNITY_EDITOR && UNITY_WEBGL
            purchase = new WebGLGamebasePurchase();
#else
            purchase = new StandaloneGamebasePurchase();
#endif
        }

        public void RequestPurchase(long itemSeq, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            purchase.RequestPurchase(itemSeq, handle);
        }


        public void RequestPurchase(string marketItemId, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            purchase.RequestPurchase(marketItemId, handle);
        }

        public void RequestItemListOfNotConsumed(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            purchase.RequestItemListOfNotConsumed(handle);
        }

        public void RequestRetryTransaction(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableRetryTransactionResult> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            purchase.RequestRetryTransaction(handle);
        }

        public void RequestItemListPurchasable(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableItem>> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            purchase.RequestItemListPurchasable(handle);
        }

        public void RequestItemListAtIAPConsole(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableItem>> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            purchase.RequestItemListAtIAPConsole(handle);
        }

        public void SetPromotionIAPHandler(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            purchase.SetPromotionIAPHandler(handle);
        }

        public void SetStoreCode(string storeCode)
        {
            purchase.SetStoreCode(storeCode);
        }

        public string GetStoreCode()
        {
            return purchase.GetStoreCode();
        }

        public void RequestActivatedPurchases(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            purchase.RequestActivatedPurchases(handle);         
        }
/*
        public void SetPurchaseUpdateListener(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.IapPurchase> callback)
        {
            GamebaseIapManager.Instance.SetPurchaseUpdateListener(callback);
        }

        public void RequestProductDetails(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.RequestProductDetails> callback)
        {
            GamebaseIapManager.Instance.RequestProductDetails(callback);
        }

        public void Purchase(string productId, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.IapPurchase> callback)
        {
            GamebaseIapManager.Instance.Purchase(productId, callback);
        }

        public void RequestConsumablePurchases(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.IapPurchase>> callback)
        {
            GamebaseIapManager.Instance.RequestConsumablePurchases(callback);
        }

        public void RequestRestorePurchases(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.IapPurchase>> callback)
        {
            GamebaseIapManager.Instance.RequestRestorePurchases(callback);
        }

        public void RequestActivatedPurchases(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.IapPurchase>> callback)
        {
            GamebaseIapManager.Instance.RequestActivatedPurchases(callback);
        }

        public void SetExtraData(Dictionary<string, string> extraData)
        {
            GamebaseIapManager.Instance.SetExtraData(extraData);
        }
*/
    }
}