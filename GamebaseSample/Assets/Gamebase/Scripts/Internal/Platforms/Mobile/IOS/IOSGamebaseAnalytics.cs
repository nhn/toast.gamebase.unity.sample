#if UNITY_EDITOR || UNITY_IOS
namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebaseAnalytics : NativeGamebaseAnalytics
    {
        override protected void Init()
        {
            CLASS_NAME      = "TCGBAnalyticsPlugin";
            messageSender   = IOSMessageSender.Instance;

            base.Init();
        }
    }
}
#endif