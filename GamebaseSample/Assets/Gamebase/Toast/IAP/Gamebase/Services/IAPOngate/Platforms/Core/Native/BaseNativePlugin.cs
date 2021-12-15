namespace Toast.Iap.Ongate
{
    abstract public class BaseNativePlugin : IAPNativePlugin, IAPService
    {
        void IAPService.Purchase(string itemId, IAPCallbackHandler.OnResponsePurchase callback)
        {
            int eventId = IAPCallbackHandler.Instance.AddCallback(callback);
            IAPEventParam param = new IAPEventParam.Builder(eventId, IAPEvent.REQUEST_PURCHASE)
                .Add("itemId", System.Convert.ToInt64(itemId)).Build();

            DebugUtil.Log("Call Native RequestPurchase = " + param.ToString());
            RequestAsyncEvent(param);
        }

        void IAPService.RequestConsumablePurchases(IAPCallbackHandler.OnResponsePurchase callback)
        {
            int eventId = IAPCallbackHandler.Instance.AddCallback(callback);
            IAPEventParam param = new IAPEventParam.Builder(eventId, IAPEvent.QUERY_PURCHASES).Build();

            DebugUtil.Log("Call Native QueryPurchases = " + param.ToString());
            RequestAsyncEvent(param);
        }

        void IAPService.RequestProductDetails(IAPCallbackHandler.OnResponsePurchase callback)
        {
            int eventId = IAPCallbackHandler.Instance.AddCallback(callback);
            IAPEventParam param = new IAPEventParam.Builder(eventId, IAPEvent.QUERY_ITEMS).Build();

            DebugUtil.Log("Call Native QueryItems = " + param.ToString());
            RequestAsyncEvent(param);
        }

        void IAPService.SetDebugMode(bool isDebuggable)
        {
            SetDebugMode(isDebuggable);
        }

        bool IAPService.SetOngateUserId(string userId)
        {
            return RegisterUserId(userId);
        }
    }
}
