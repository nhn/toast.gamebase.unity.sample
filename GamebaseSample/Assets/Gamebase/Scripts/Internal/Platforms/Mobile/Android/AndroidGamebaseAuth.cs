#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebaseAuth : NativeGamebaseAuth
    {
        override protected void Init()
        {
            CLASS_NAME      = "com.toast.android.gamebase.plugin.GamebaseAuthPlugin";
            messageSender   = AndroidMessageSender.Instance;

            base.Init();
        }
    }
}
#endif