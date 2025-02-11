#if UNITY_EDITOR || UNITY_IOS
using System;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebaseUtil : NativeGamebaseUtil
    {
        override protected void Init()
        {
            CLASS_NAME      = "TCGBUtilPlugin";
            messageSender   = IOSMessageSender.Instance;
            
            base.Init();
        }
        override public GamebaseAppTrackingAuthorizationStatus GetAppTrackingAuthorizationStatus()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseUtil.UTIL_API_APP_TRACKING_AUTHORIZATION_STATUS));
            string jsonString = messageSender.GetSync(jsonData);

            if (string.IsNullOrEmpty(jsonString) == true)
            {
                return GamebaseAppTrackingAuthorizationStatus.UNKNOWN;
            }
            else
            {
                return (GamebaseAppTrackingAuthorizationStatus)Convert.ToInt32(jsonString);
            }
            
        }
    }
}
#endif