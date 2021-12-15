#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebaseUtil : NativeGamebaseUtil
    {
        override protected void Init()
        {
            CLASS_NAME      = "com.toast.android.gamebase.plugin.GamebaseUtilPlugin";
            messageSender   = AndroidMessageSender.Instance;

            base.Init();
        }
    }
}
#endif