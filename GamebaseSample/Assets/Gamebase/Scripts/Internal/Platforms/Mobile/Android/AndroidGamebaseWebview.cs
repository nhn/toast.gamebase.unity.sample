#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebaseWebview : NativeGamebaseWebview
    {
        override protected void Init()
        {
            CLASS_NAME      = "com.toast.android.gamebase.unityplugin.GamebaseWebviewPlugin";
            messageSender   = AndroidMessageSender.Instance;
            
            base.Init();
        }
    }
}
#endif