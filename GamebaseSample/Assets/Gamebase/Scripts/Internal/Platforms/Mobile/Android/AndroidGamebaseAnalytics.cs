#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebaseAnalytics : NativeGamebaseAnalytics
    {
        override protected void Init()
        {
            CLASS_NAME      = "com.toast.android.gamebase.plugin.GamebaseAnalyticsPlugin";
            messageSender   = AndroidMessageSender.Instance;

            base.Init();
        }
    }
}
#endif