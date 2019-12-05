#if UNITY_EDITOR || UNITY_STANDALONE

using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseUtil : CommonGamebaseUtil
    {
        private const int BUTTON_OK         = 1;
        private const int BUTTON_CANCEL     = 2;

        public StandaloneGamebaseUtil()
        {
            Domain = typeof(StandaloneGamebaseUtil).Name;
        }

        public override void ShowAlert(string title, string message)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            StandaloneGamebaseMessageBox.ShowMessageBox(title, message, StandaloneGamebaseMessageBox.MB_OK);
#else
            base.ShowAlert(title, message);
#endif
        }

        public override void ShowAlert(string title, string message, int handle)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            StandaloneGamebaseMessageBox.ShowMessageBox(title, message, StandaloneGamebaseMessageBox.MB_OK);

            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.VoidDelegate>(handle);
            if (null != callback)
            {
                callback();
            }
#else
            base.ShowAlert(title, message, handle);
#endif
        }
        
        public override void ShowAlert(Dictionary<string, string> parameters, GamebaseUtilAlertType alertType, int handle)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            string title        = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_TITLE);
            string message      = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_MESSAGE);
            string buttonLeft   = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_BUTTON_LEFT);
            string buttonRight  = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_BUTTON_RIGHT);

            int select = BUTTON_OK;

            if (GamebaseUtilAlertType.ALERT_OK == alertType)
            {
                StandaloneGamebaseMessageBox.OK = buttonRight;

                select = StandaloneGamebaseMessageBox.ShowMessageBox(
                    title,
                    message,
                    StandaloneGamebaseMessageBox.MB_OK);
            }
            else
            {
                StandaloneGamebaseMessageBox.OK = buttonLeft;
                StandaloneGamebaseMessageBox.Cancel = buttonRight;

                select = StandaloneGamebaseMessageBox.ShowMessageBox(
                    title,
                    message,
                    StandaloneGamebaseMessageBox.MB_OKCANCEL);
            }

            GamebaseUtilAlertButtonID buttonID;
            if (BUTTON_OK == select)
            {
                buttonID = GamebaseUtilAlertButtonID.BUTTON_ONE;
            }
            else
            {
                buttonID = GamebaseUtilAlertButtonID.BUTTON_TWO;
            }

            if (-1 != handle)
            {
                GamebaseCallback.DataDelegate<GamebaseUtilAlertButtonID> callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.DataDelegate<GamebaseUtilAlertButtonID>>(handle);
                if (null != callback)
                {
                    callback(buttonID);
                }
            }            
#else
            base.ShowAlert(parameters, alertType, handle);
#endif
        }
    }
}
#endif