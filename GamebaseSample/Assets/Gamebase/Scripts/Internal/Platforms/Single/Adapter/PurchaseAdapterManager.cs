#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;

namespace Toast.Gamebase.Internal.Single
{
    public class PurchaseAdapterManager
    {
        private static readonly PurchaseAdapterManager instance = new PurchaseAdapterManager();

        public static PurchaseAdapterManager Instance
        {
            get { return instance; }
        }

        public IPurchaseAdapter adapter;

        public bool CreateIDPAdapter(string moduleName)
        {
            if (adapter != null)
            {
                return true;
            }

            adapter = AdapterFactory.CreateAdapter<IPurchaseAdapter>(moduleName);

            if (adapter == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public void Initialize()
        {
            if (adapter == null)
            {
                GamebaseLog.Debug(GamebaseStrings.PURCHASE_ADAPTER_NOT_FOUND, this);                
                return;
            }

            adapter.Initialize();
        }

        public void SetConfiguration(PurchaseRequest.Configuration iapConfiguration)
        {
            if (adapter == null)
            {
                GamebaseLog.Debug(GamebaseStrings.PURCHASE_ADAPTER_NOT_FOUND, this);
                return;
            }

            adapter.SetConfiguration(iapConfiguration);
        }

        public void SetExtraData(Dictionary<string, string> extraData)
        {
            if (adapter == null)
            {
                GamebaseLog.Debug(GamebaseStrings.PURCHASE_ADAPTER_NOT_FOUND, this);
                return;
            }

            adapter.SetExtraData(extraData);
        }

        public void RequestPurchase(long itemSeq, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt> callback)
        {
            if (adapter == null)
            {
                callback(null, new GamebaseError(GamebaseErrorCode.PURCHASE_UNKNOWN_ERROR, message: GamebaseStrings.PURCHASE_ADAPTER_NOT_FOUND));
                return;
            }

            adapter.RequestPurchase(itemSeq, callback);
        }

        public void RequestItemListPurchasable(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableItem>> callback)
        {
            if (adapter == null)
            {
                callback(null, new GamebaseError(GamebaseErrorCode.PURCHASE_UNKNOWN_ERROR, message: GamebaseStrings.PURCHASE_ADAPTER_NOT_FOUND));
                return;
            }

            adapter.RequestItemListPurchasable(callback);
        }

        public void RequestItemListAtIAPConsole(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableItem>> callback)
        {
            if (adapter == null)
            {
                callback(null, new GamebaseError(GamebaseErrorCode.PURCHASE_UNKNOWN_ERROR, message: GamebaseStrings.PURCHASE_ADAPTER_NOT_FOUND));
                return;
            }

            adapter.RequestItemListAtIAPConsole(callback);
        }

        public void RequestItemListOfNotConsumed(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>> callback)
        {
            if (adapter == null)
            {
                callback(null, new GamebaseError(GamebaseErrorCode.PURCHASE_UNKNOWN_ERROR, message: GamebaseStrings.PURCHASE_ADAPTER_NOT_FOUND));
                return;
            }

            adapter.RequestItemListOfNotConsumed(callback);
        }

        public void RequestRetryTransaction(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableRetryTransactionResult> callback)
        {
            if (adapter == null)
            {
                callback(null, new GamebaseError(GamebaseErrorCode.PURCHASE_UNKNOWN_ERROR, message: GamebaseStrings.PURCHASE_ADAPTER_NOT_FOUND));
                return;
            }

            adapter.RequestRetryTransaction(callback);
        }

        public void Destroy()
        {
            if (adapter == null)
            {
                return;
            }

            adapter.Destroy();
        }
    }
}
#endif