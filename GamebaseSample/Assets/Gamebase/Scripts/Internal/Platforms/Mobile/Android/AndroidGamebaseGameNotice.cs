#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebaseGameNotice : NativeGamebaseGameNotice
    {
        override protected void Init()
        {
            CLASS_NAME = "com.toast.android.gamebase.plugin.GamebaseGameNoticePlugin";
            messageSender = AndroidMessageSender.Instance;

            base.Init();
        }
    }
}
#endif