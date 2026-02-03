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

        override public string GetIdfa()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseUtil.UTIL_API_IDFA));
            return messageSender.GetSync(jsonData);
        }

        override public void GetAgeRangeService(int ageGates, int? threshold2, int? threshold3, int handle)
        {
            var vo          = new NativeRequest.Util.AgeRange();
            vo.ageGates     = ageGates;
            vo.threshold2   = threshold2;
            vo.threshold3   = threshold3;

            string jsonData     = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseUtil.UTIL_API_GET_AGE_RANGE,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(vo)
                ));
            
            messageSender.GetAsync(jsonData);
        }
    }
}
#endif