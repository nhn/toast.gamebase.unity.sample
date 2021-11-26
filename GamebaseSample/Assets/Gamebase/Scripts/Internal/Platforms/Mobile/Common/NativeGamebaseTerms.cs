#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseTerms : IGamebaseTerms
    {
        protected class GamebaseTerms
        {
            public const string TERMS_API_SHOW_TERMS_VIEW = "gamebase://showTermsView";
            public const string TERMS_API_UPDATE_TERMS = "gamebase://updateTerms";
            public const string TERMS_API_QUERY_TERMS = "gamebase://queryTerms";
        }

        protected INativeMessageSender messageSender = null;
        protected string CLASS_NAME = string.Empty;

        public NativeGamebaseTerms()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);
            messageSender.InitializeUnityInterface();

            DelegateManager.AddDelegate(GamebaseTerms.TERMS_API_SHOW_TERMS_VIEW, DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.DataContainer>);
            DelegateManager.AddDelegate(GamebaseTerms.TERMS_API_UPDATE_TERMS, DelegateManager.SendErrorDelegateOnce);
            DelegateManager.AddDelegate(GamebaseTerms.TERMS_API_QUERY_TERMS, DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Terms.QueryTermsResult>);
            
        }

        public void ShowTermsView(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseTerms.TERMS_API_SHOW_TERMS_VIEW,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void UpdateTerms(GamebaseRequest.Terms.UpdateTermsConfiguration configuration, int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseTerms.TERMS_API_UPDATE_TERMS,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(configuration)));
            messageSender.GetAsync(jsonData);
        }

        public void QueryTerms(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseTerms.TERMS_API_QUERY_TERMS,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }
    }
}
#endif
