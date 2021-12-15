#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;

namespace Toast.Gamebase.Internal.Single
{
    public interface IPurchaseAdapter
    {
        void Initialize();
        void SetConfiguration(PurchaseRequest.Configuration iapConfiguration);
        void SetExtraData(Dictionary<string, string> extraData);

        void RequestPurchase(long itemSeq, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt> callback);
        void RequestItemListPurchasable(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableItem>> callback);
        void RequestItemListAtIAPConsole(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableItem>> callback);
        void RequestItemListOfNotConsumed(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>> callback);
        void RequestRetryTransaction(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableRetryTransactionResult> callback);

        void Destroy();
    }
}
#endif