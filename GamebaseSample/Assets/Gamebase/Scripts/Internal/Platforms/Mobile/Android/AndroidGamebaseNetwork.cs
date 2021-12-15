#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebaseNetwork : NativeGamebaseNetwork
    {
        override protected void Init()
        {
            CLASS_NAME      = "com.toast.android.gamebase.plugin.GamebaseNetworkPlugin";
            messageSender   = AndroidMessageSender.Instance;

            base.Init();
        }
    }
}
#endif