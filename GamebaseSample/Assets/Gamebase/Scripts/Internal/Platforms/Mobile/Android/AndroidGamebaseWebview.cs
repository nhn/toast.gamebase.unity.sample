#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebaseWebview : NativeGamebaseWebview
    {
        override protected void Init()
        {
            CLASS_NAME      = "com.toast.android.gamebase.plugin.GamebaseWebviewPlugin";
            messageSender   = AndroidMessageSender.Instance;
            
            base.Init();
        }
    }
}
#endif