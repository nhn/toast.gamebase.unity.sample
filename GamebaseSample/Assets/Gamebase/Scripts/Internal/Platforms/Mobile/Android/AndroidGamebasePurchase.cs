#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebasePurchase : NativeGamebasePurchase
    {
        override protected void Init()
        {
            CLASS_NAME      = "com.toast.android.gamebase.plugin.GamebasePurchasePlugin";
            messageSender   = AndroidMessageSender.Instance;

            base.Init();
        }

        override public void SetPromotionIAPHandler(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt>>(handle));
        }
    }
}
#endif