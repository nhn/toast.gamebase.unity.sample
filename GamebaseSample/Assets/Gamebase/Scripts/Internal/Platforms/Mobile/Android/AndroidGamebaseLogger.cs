#if UNITY_EDITOR || UNITY_ANDROID
using Toast.Gamebase.Internal.Mobile.Android;

namespace Toast.Gamebase.Mobile.Android
{
    public class AndroidGamebaseLogger : NativeGamebaseLogger
    {
        override protected void Init()
        {
            CLASS_NAME = "com.toast.android.gamebase.plugin.GamebaseLoggerPlugin";
            messageSender = AndroidMessageSender.Instance;

            base.Init();
        }
    }
}
#endif