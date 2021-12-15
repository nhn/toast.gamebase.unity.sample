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

    public sealed class GamebaseImageNoticeImplementation
    {

        private static readonly GamebaseImageNoticeImplementation instance = new GamebaseImageNoticeImplementation();

        public static GamebaseImageNoticeImplementation Instance
        {
            get { return instance; }
        }

        IGamebaseImageNotice imageNotice;

        private GamebaseImageNoticeImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            imageNotice = new AndroidGamebaseImageNotice();
#elif !UNITY_EDITOR && UNITY_IOS
            imageNotice = new IOSGamebaseImageNotice();
#elif !UNITY_EDITOR && UNITY_WEBGL
            imageNotice = new WebGLGamebaseImageNotice();
#else
            imageNotice = new StandaloneGamebaseImageNotice();
#endif
        }

        public void ShowImageNotices(GamebaseRequest.ImageNotice.Configuration configuration, GamebaseCallback.ErrorDelegate closeCallback, GamebaseCallback.GamebaseDelegate<string> eventCallback)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            int closeHandle = GamebaseCallbackHandler.RegisterCallback(closeCallback);
            int eventHandle = -1;
            if (eventCallback != null)
            {
                eventHandle = GamebaseCallbackHandler.RegisterCallback(eventCallback);
            }

            imageNotice.ShowImageNotices(configuration, closeHandle, eventHandle);
        }

        public void CloseImageNotices()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            imageNotice.CloseImageNotices();
        }
    }
}
