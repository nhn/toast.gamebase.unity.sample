#if UNITY_EDITOR || UNITY_ANDROID

using Toast.Gamebase.LitJson;

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
        
        override public void GetAgeSignal(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseUtil.UTIL_API_GET_AGE_SIGNAL,
                    handle: handle
                ));
            messageSender.GetAsync(jsonData);
        }
    }
}
#endif