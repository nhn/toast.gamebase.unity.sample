#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebaseLaunching : NativeGamebaseLaunching
    {
         protected override void Init()
        {
            CLASS_NAME      = "com.toast.android.gamebase.plugin.GamebaseLaunchingPlugin";
            messageSender   = AndroidMessageSender.Instance;

            base.Init();
        }
    }
}
#endif