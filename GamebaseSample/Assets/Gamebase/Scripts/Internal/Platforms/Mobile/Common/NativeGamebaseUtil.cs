#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseUtil : IGamebaseUtil
    {
        protected class GamebaseUtil
        {            
            public const string UTIL_API_SHOW_TOAST_WITH_TYPE               = "gamebase://showToastWithType";
            public const string UTIL_API_SHOW_ALERT                         = "gamebase://showAlert";
            public const string UTIL_API_SHOW_ALERT_EVENT                   = "gamebase://showAlertEvent";
            public const string UTIL_API_APP_TRACKING_AUTHORIZATION_STATUS  = "gamebase://appTrackingAuthorizationStatus";
            public const string UTIL_API_IDFA                               = "gamebase://idfa";
            public const string UTIL_API_GET_AGE_SIGNAL                     = "gamebase://getAgeSignal";
            public const string UTIL_API_GET_AGE_RANGE                      = "gamebase://getAgeRange";
        }

        protected INativeMessageSender  messageSender           = null;
        protected string                CLASS_NAME              = "TCGBUtilPlugin";

        public NativeGamebaseUtil()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);

            DelegateManager.AddDelegate(GamebaseUtil.UTIL_API_SHOW_ALERT_EVENT, DelegateManager.SendVoidDelegateOnce);
            DelegateManager.AddDelegate(GamebaseUtil.UTIL_API_GET_AGE_SIGNAL, DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Util.AgeSignalResult>);
        }
        
        virtual public void ShowToast(string message, GamebaseUIToastType type)
        {
            NativeRequest.Util.AlertDialog alertDialog  = new NativeRequest.Util.AlertDialog();
            alertDialog.message                         = message;
            alertDialog.duration                        = (int)type;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseUtil.UTIL_API_SHOW_TOAST_WITH_TYPE, 
                    jsonData: JsonMapper.ToJson(alertDialog)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void ShowAlert(string title, string message)
        {
            NativeRequest.Util.AlertDialog alertDialog  = new NativeRequest.Util.AlertDialog();
            alertDialog.title                           = title;
            alertDialog.message                         = message;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseUtil.UTIL_API_SHOW_ALERT, 
                    jsonData: JsonMapper.ToJson(alertDialog)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void ShowAlert(string title, string message, int handle)
        {
            NativeRequest.Util.AlertDialog alertDialog  = new Mobile.NativeRequest.Util.AlertDialog();
            alertDialog.title                           = title;
            alertDialog.message                         = message;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseUtil.UTIL_API_SHOW_ALERT_EVENT, 
                    handle: handle, 
                    jsonData: JsonMapper.ToJson(alertDialog)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void ShowAlert(Dictionary<string, string> parameters, GamebaseUtilAlertType alertType, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
        }

        virtual public GamebaseAppTrackingAuthorizationStatus GetAppTrackingAuthorizationStatus()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return GamebaseAppTrackingAuthorizationStatus.UNKNOWN;
        }

        public virtual string GetIdfa()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return "";
        }
        
        virtual public void GetAgeSignal(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
        }

        virtual public void GetAgeRangeService(int ageGates, int? threshold2, int? threshold3, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
        }
    }
}
#endif