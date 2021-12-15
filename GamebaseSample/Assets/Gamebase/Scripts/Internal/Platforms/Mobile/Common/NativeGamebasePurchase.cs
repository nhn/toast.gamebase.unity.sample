#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebasePurchase : IGamebasePurchase
    {
        protected class GamebasePurchase
        {
            public const string PURCHASE_API_REQUEST_PURCHASE_SEQ                       = "gamebase://requestPurchaseSeq";
            public const string PURCHASE_API_REQUEST_PURCHASE_PRODUCTID                 = "gamebase://requestPurchaseProductId";
            public const string PURCHASE_API_REQUEST_PURCHASE_PRODUCTID_WITH_PAYLOAD    = "gamebase://requestPurchaseProductIdWithPayload";
            public const string PURCHASE_API_REQUEST_ITEM_LIST_OF_NOT_CONSUMED          = "gamebase://requestItemListOfNotConsumed";
            public const string PURCHASE_API_REQUEST_RETYR_TRANSACTION                  = "gamebase://requestRetryTransaction";
            public const string PURCHASE_API_REQUEST_ITEM_LIST_PURCHASABLE              = "gamebase://requestItemListPurchasable";
            public const string PURCHASE_API_REQUEST_ITEM_LIST_AT_AP_CONSOLE            = "gamebase://requestItemListAtIAPConsole";
            public const string PURCHASE_API_SET_PROMOTION_IAP_HANDLER                  = "gamebase://setPromotionIAPHandler";
            public const string PURCHASE_API_SET_STORE_CODE                             = "gamebase://setStoreCode";
            public const string PURCHASE_API_GET_STORE_CODE                             = "gamebase://getStoreCode";
            public const string PURCHASE_API_REQUEST_ACTIVATED_PURCHASES                = "gamebase://requestActivatedPurchases";
        }

        protected INativeMessageSender  messageSender   = null;
        protected string                CLASS_NAME      = string.Empty;

        public NativeGamebasePurchase()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);

            DelegateManager.AddDelegate(GamebasePurchase.PURCHASE_API_REQUEST_PURCHASE_SEQ,                     DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Purchase.PurchasableReceipt>);
            DelegateManager.AddDelegate(GamebasePurchase.PURCHASE_API_REQUEST_PURCHASE_PRODUCTID,               DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Purchase.PurchasableReceipt>);
            DelegateManager.AddDelegate(GamebasePurchase.PURCHASE_API_REQUEST_PURCHASE_PRODUCTID_WITH_PAYLOAD,  DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Purchase.PurchasableReceipt>);
            DelegateManager.AddDelegate(GamebasePurchase.PURCHASE_API_REQUEST_ITEM_LIST_OF_NOT_CONSUMED,        DelegateManager.SendGamebaseDelegateOnce<List<GamebaseResponse.Purchase.PurchasableReceipt>>);
            DelegateManager.AddDelegate(GamebasePurchase.PURCHASE_API_REQUEST_RETYR_TRANSACTION,                DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Purchase.PurchasableRetryTransactionResult>);
            DelegateManager.AddDelegate(GamebasePurchase.PURCHASE_API_REQUEST_ITEM_LIST_PURCHASABLE,            DelegateManager.SendGamebaseDelegateOnce<List<GamebaseResponse.Purchase.PurchasableItem>>);
            DelegateManager.AddDelegate(GamebasePurchase.PURCHASE_API_REQUEST_ITEM_LIST_AT_AP_CONSOLE,          DelegateManager.SendGamebaseDelegateOnce<List<GamebaseResponse.Purchase.PurchasableItem>>);
            DelegateManager.AddDelegate(GamebasePurchase.PURCHASE_API_SET_PROMOTION_IAP_HANDLER,                DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Purchase.PurchasableReceipt>);
            DelegateManager.AddDelegate(GamebasePurchase.PURCHASE_API_REQUEST_ACTIVATED_PURCHASES,              DelegateManager.SendGamebaseDelegateOnce<List<GamebaseResponse.Purchase.PurchasableReceipt>>);
        }

        public void RequestPurchase(long itemSeq, int handle)
        {
            NativeRequest.Purchase.PurchaseItemSeq vo = new NativeRequest.Purchase.PurchaseItemSeq();
            vo.itemSeq = itemSeq;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePurchase.PURCHASE_API_REQUEST_PURCHASE_SEQ,
                    jsonData: JsonMapper.ToJson(vo),
                    handle:handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void RequestPurchase(string gamebaseProductId, int handle)
        {
            NativeRequest.Purchase.PurchaseProductId vo = new NativeRequest.Purchase.PurchaseProductId();
            vo.gamebaseProductId = gamebaseProductId;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePurchase.PURCHASE_API_REQUEST_PURCHASE_PRODUCTID,
                    jsonData: JsonMapper.ToJson(vo),
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void RequestPurchase(string gamebaseProductId, string payload, int handle)
        {
            NativeRequest.Purchase.PurchaseProductId vo = new NativeRequest.Purchase.PurchaseProductId();
            vo.gamebaseProductId = gamebaseProductId;
            vo.payload = payload;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePurchase.PURCHASE_API_REQUEST_PURCHASE_PRODUCTID_WITH_PAYLOAD,
                    jsonData: JsonMapper.ToJson(vo),
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void RequestItemListOfNotConsumed(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePurchase.PURCHASE_API_REQUEST_ITEM_LIST_OF_NOT_CONSUMED,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void RequestRetryTransaction(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(GamebasePurchase.PURCHASE_API_REQUEST_RETYR_TRANSACTION,
                handle: handle
                ));
            messageSender.GetAsync(jsonData);
        }

        public void RequestItemListPurchasable(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(GamebasePurchase.PURCHASE_API_REQUEST_ITEM_LIST_PURCHASABLE,
                handle: handle
                ));
            messageSender.GetAsync(jsonData);
        }

        public void RequestItemListAtIAPConsole(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePurchase.PURCHASE_API_REQUEST_ITEM_LIST_AT_AP_CONSOLE,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void SetPromotionIAPHandler(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePurchase.PURCHASE_API_SET_PROMOTION_IAP_HANDLER,
                    handle: handle
                    ));

            messageSender.GetAsync(jsonData);
        }

        public void SetStoreCode(string storeCode)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePurchase.PURCHASE_API_SET_STORE_CODE,
                    jsonData: storeCode
                    ));
            messageSender.GetSync(jsonData);
        }

        public string GetStoreCode()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebasePurchase.PURCHASE_API_GET_STORE_CODE));
            return messageSender.GetSync(jsonData);
        }

        public void RequestActivatedPurchases(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePurchase.PURCHASE_API_REQUEST_ACTIVATED_PURCHASES,
                    handle: handle
                    ));

            messageSender.GetAsync(jsonData);
        }
    }
}
#endif