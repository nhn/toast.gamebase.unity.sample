#if UNITY_EDITOR || UNITY_WEBGL

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseUtil : CommonGamebaseUtil
    {
        private const int BUTTON_OK         = 1;
        private const int BUTTON_CANCEL     = 2;

        [DllImport("__Internal")]
        private extern static void Alert(string title, string message);

        [DllImport("__Internal")]
        private extern static int Confirm(string title, string message);
        
        public WebGLGamebaseUtil()
        {
            Domain = typeof(WebGLGamebaseUtil).Name;
        }

        public override void ShowAlert(string title, string message)
        {
            Alert(title, message);
        }

        public override void ShowAlert(string title, string message, int handle)
        {
            Alert(title, message);

            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.VoidDelegate>(handle);
            if (null != callback)
            {
                callback();
            }
        }

        public override void ShowAlert(Dictionary<string, string> parameters, GamebaseUtilAlertType alertType, int handle)
        {
            string title    = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_TITLE);
            string message  = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_MESSAGE);
            string extra    = GetDictionaryValue(parameters, GamebaseSystemPopup.KEY_EXTRA);

            int select = BUTTON_OK;

            if (GamebaseUtilAlertType.ALERT_OK == alertType)
            {
                StringBuilder sb = new StringBuilder(message);

                Alert(title, sb.ToString());
            }
            else
            {
                StringBuilder sb = new StringBuilder(message);
                if (false == string.IsNullOrEmpty(extra))
                {
                    sb.Append("\n\n").Append(extra);
                }

                select = Confirm(title, sb.ToString());
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
        }
    }
}
#endif