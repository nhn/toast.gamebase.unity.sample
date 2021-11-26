namespace Toast.Iap.Ongate
{
    public interface IAPService
    {
        void SetDebugMode(bool isDebuggable);

        bool SetOngateUserId(string userId);

        void Purchase(string itemId, IAPCallbackHandler.OnResponsePurchase callback);

        void RequestConsumablePurchases(IAPCallbackHandler.OnResponsePurchase callback);

        void RequestProductDetails(IAPCallbackHandler.OnResponsePurchase callback);
    }
}