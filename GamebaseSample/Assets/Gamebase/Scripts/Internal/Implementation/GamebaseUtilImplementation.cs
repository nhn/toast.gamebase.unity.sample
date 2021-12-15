#if !UNITY_EDITOR && UNITY_ANDROID
using Toast.Gamebase.Internal.Mobile.Android;
#elif !UNITY_EDITOR && UNITY_IOS
using Toast.Gamebase.Internal.Mobile.IOS;
#elif !UNITY_EDITOR && UNITY_WEBGL
using Toast.Gamebase.Internal.Single.WebGL;
#else
using Toast.Gamebase.Internal.Single.Standalone;
#endif
using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseUtilImplementation
    {
        private static readonly GamebaseUtilImplementation instance = new GamebaseUtilImplementation();
        
        public static GamebaseUtilImplementation Instance
        {
            get { return instance; }
        }

        IGamebaseUtil util;

        private GamebaseUtilImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            util = new AndroidGamebaseUtil();
#elif !UNITY_EDITOR && UNITY_IOS
            util = new IOSGamebaseUtil();
#elif !UNITY_EDITOR && UNITY_WEBGL
            util = new WebGLGamebaseUtil();
#else
            util = new StandaloneGamebaseUtil();
#endif
        }

        public void ShowAlert(string title, string message)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            util.ShowAlert(title, message);
        }

        public void ShowAlert(string title, string message, GamebaseCallback.VoidDelegate buttonCallback)
        {
            GamebaseGameInformationReport.Instance.AddApiName("ShowAlertWithButtonCallback");
            int handle = GamebaseCallbackHandler.RegisterCallback(buttonCallback);

            util.ShowAlert(title, message, handle);
        }
        
        public void ShowToast(string message, GamebaseUIToastType type)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            util.ShowToast(message, type);
        }
        
        public void ShowAlert(Dictionary<string, string> parameters, GamebaseUtilAlertType alertType, GamebaseCallback.DataDelegate<GamebaseUtilAlertButtonID> buttonCallback)
        {
            GamebaseGameInformationReport.Instance.AddApiName("ShowAlertWithParametersAndAlertTypeAndButtonCallback");
            int handle = GamebaseCallbackHandler.RegisterCallback(buttonCallback);
            util.ShowAlert(parameters, alertType, handle);
        }
    }
}