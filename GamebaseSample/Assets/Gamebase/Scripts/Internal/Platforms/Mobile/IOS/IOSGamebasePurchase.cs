#if UNITY_EDITOR || UNITY_IOS
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebasePurchase : NativeGamebasePurchase
    {
        override protected void Init()
        {
            CLASS_NAME      = "TCGBPurchasePlugin";
            messageSender   = IOSMessageSender.Instance;

            base.Init();
        }

        public override void RequestSubscriptionsStatus(GamebaseRequest.Purchase.PurchasableConfiguration configuration, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate
                <List<GamebaseResponse.Purchase.PurchasableSubscriptionStatus>>>(handle));
        }
    }
}
#endif