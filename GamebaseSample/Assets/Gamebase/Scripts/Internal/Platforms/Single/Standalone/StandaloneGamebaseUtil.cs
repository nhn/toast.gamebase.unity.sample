#if UNITY_EDITOR || UNITY_STANDALONE

using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseUtil : CommonGamebaseUtil
    {
        private const int BUTTON_OK         = 1;
        private const int BUTTON_CANCEL     = 2;

        private List<string> DefaultOkButtonList
        {
            get
            {
                return new List<string>
                {
                    DisplayLanguage.Instance.GetString("common_ok_button")
                };
            }
        }
        
        private List<string> DefaultOkCancelButtonList
        {
            get
            {
                return new List<string>
                {
                    DisplayLanguage.Instance.GetString("common_ok_button"),
                    DisplayLanguage.Instance.GetString("common_cancel_button")
                };
            }
        }

        public StandaloneGamebaseUtil()
        {
            Domain = typeof(StandaloneGamebaseUtil).Name;
        }

        public override void ShowAlert(string title, string message)
        {
            GamebaseNativeUtils.Instance.ShowPopup(new GamebasePopupInfo(title, message, DefaultOkButtonList));
        }

        public override void ShowAlert(string title, string message, int handle)
        {
            GamebaseNativeUtils.Instance.ShowPopup(new GamebasePopupInfo(title, message, DefaultOkButtonList));
            
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.VoidDelegate>(handle);
            if (callback != null)
            {
                callback();
            }
        }
        
        public override void ShowAlert(Dictionary<string, string> parameters, GamebaseUtilAlertType alertType, int handle)
        {
            string title        = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_TITLE);
            string message      = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_MESSAGE);
            string buttonLeft   = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_BUTTON_LEFT);
            string buttonRight  = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_BUTTON_RIGHT);

            int resultIndex;
            if (alertType == GamebaseUtilAlertType.ALERT_OKCANCEL)
            {
                resultIndex = GamebaseNativeUtils.Instance.ShowPopup(new GamebasePopupInfo(title, message, new List<string>
                {
                    buttonLeft,
                    buttonRight
                }));
            }
            else
            {
                resultIndex = GamebaseNativeUtils.Instance.ShowPopup(new GamebasePopupInfo(title, message, new List<string>
                {
                    buttonRight
                }));
            }

            var buttonID = resultIndex == BUTTON_OK ? GamebaseUtilAlertButtonID.BUTTON_ONE : GamebaseUtilAlertButtonID.BUTTON_TWO;
            
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.DataDelegate<GamebaseUtilAlertButtonID>>(handle);
            if (callback != null)
            {
                callback(buttonID);
            }
        }
    }
}
#endif