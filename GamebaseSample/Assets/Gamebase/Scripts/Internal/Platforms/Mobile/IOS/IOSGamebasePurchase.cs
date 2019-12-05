#if UNITY_EDITOR || UNITY_IOS
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
    }
}
#endif