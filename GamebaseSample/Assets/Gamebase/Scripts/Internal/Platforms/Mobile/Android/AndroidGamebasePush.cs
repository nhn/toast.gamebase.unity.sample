#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebasePush : NativeGamebasePush
    {
        override protected void Init()
        {
            CLASS_NAME      = "com.toast.android.gamebase.plugin.GamebasePushPlugin";
            messageSender   = AndroidMessageSender.Instance;
            
            base.Init();
        }

        override public void SetSandboxMode(bool isSandbox)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
        }
    }
}
#endif