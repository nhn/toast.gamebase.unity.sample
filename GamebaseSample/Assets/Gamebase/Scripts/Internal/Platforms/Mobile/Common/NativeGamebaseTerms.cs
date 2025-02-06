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
            public const string TERMS_API_SHOW_TERMS_VIEW_WITH_CONFIGURATION = "gamebase://showTermsViewWithConfiguration";
            public const string TERMS_API_UPDATE_TERMS = "gamebase://updateTerms";
            public const string TERMS_API_QUERY_TERMS = "gamebase://queryTerms";
            public const string TERMS_IS_SHOWING_TERMS_VIEW = "gamebase://isShowingTermsView";
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

            DelegateManager.AddDelegate(GamebaseTerms.TERMS_API_SHOW_TERMS_VIEW_WITH_CONFIGURATION, DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.DataContainer>);
            DelegateManager.AddDelegate(GamebaseTerms.TERMS_API_UPDATE_TERMS, DelegateManager.SendErrorDelegateOnce);
            DelegateManager.AddDelegate(GamebaseTerms.TERMS_API_QUERY_TERMS, DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Terms.QueryTermsResult>);   
        }
        
        public void ShowTermsView(GamebaseRequest.Terms.GamebaseTermsConfiguration configuration, int handle)
        {
            string jsonString = null;
            if (configuration != null)
            {
                jsonString = JsonMapper.ToJson(configuration);
            }

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseTerms.TERMS_API_SHOW_TERMS_VIEW_WITH_CONFIGURATION,
                    handle: handle,
                    jsonData: jsonString
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

        public bool IsShowingTermsView()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseTerms.TERMS_IS_SHOWING_TERMS_VIEW));
            string jsonString = messageSender.GetSync(jsonData);

            if (string.IsNullOrEmpty(jsonString) == true)
            {
                return false;
            }

            JsonData requestData = JsonMapper.ToObject(jsonString);

            if (null != requestData["isShowingTermsView"])
            {
                return (bool)requestData["isShowingTermsView"];
            }

            return false;
        }
    }
}
#endif
