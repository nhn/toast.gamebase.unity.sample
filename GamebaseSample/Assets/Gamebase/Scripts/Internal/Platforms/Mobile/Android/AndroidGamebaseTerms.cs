#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
	public class AndroidGamebaseTerms : NativeGamebaseTerms
	{
        override protected void Init()
        {
            CLASS_NAME = "com.toast.android.gamebase.plugin.GamebaseTermsPlugin";
            messageSender = AndroidMessageSender.Instance;

            base.Init();
        }
    }
}
#endif
