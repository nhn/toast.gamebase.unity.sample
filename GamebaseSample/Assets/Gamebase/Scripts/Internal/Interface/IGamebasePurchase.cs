namespace Toast.Gamebase.Internal
{
    internal interface IGamebasePurchase
    {
        void RequestPurchase(long itemSeq, int handle);
        void RequestPurchase(string marketItemId, int handle);
        void RequestItemListOfNotConsumed(int handle);
        void RequestRetryTransaction(int handle);
        void RequestItemListPurchasable(int handle);
        void RequestItemListAtIAPConsole(int handle);
        void SetPromotionIAPHandler(int handle);
        void SetStoreCode(string storeCode);
        string GetStoreCode();
        void RequestActivatedPurchases(int handle);
    }
}