#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseTerms : IGamebaseTerms
    {
        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain) == true)
                {
                    return typeof(CommonGamebase).Name;
                }
                    

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public void ShowTermsView(GamebaseRequest.Terms.GamebaseTermsConfiguration configuration, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.DataContainer>>(handle));
        }
        
        public void UpdateTerms(GamebaseRequest.Terms.UpdateTermsConfiguration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
            GamebaseCallbackHandler.UnregisterCallback(handle);

            if (configuration == null)
            {
                if (callback != null)
                {
                    callback(new GamebaseError(GamebaseErrorCode.INVALID_PARAMETER, Domain));
                }
                return;
            }

            if (GamebaseUnitySDK.IsInitialized == false)
            {
                if (callback != null)
                {
                    callback(new GamebaseError(GamebaseErrorCode.NOT_INITIALIZED, Domain));
                }                
                return;
            }

            var requestVo = TermsMessage.GetUpdateTermsMessage(configuration);

            if (requestVo == null)
            {
                if (callback != null)
                {
                    callback(new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                }
                return;
            }

            WebSocket.Instance.Request(requestVo, (response, error) =>
            {
                if (error != null)
                {
                    if (callback != null)
                    {
                        callback(error);
                    }
                    return;
                }

                var vo = JsonMapper.ToObject<TermsResponse.UpdateTerms>(response);
                if (vo.header.isSuccessful == false)
                {
                    error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVo.transactionId, requestVo.apiId, vo.header, Domain);
                }

                if (callback != null)
                {
                    callback(error);
                }   
            });
        }

        public void QueryTerms(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Terms.QueryTermsResult>>(handle);
            GamebaseCallbackHandler.UnregisterCallback(handle);

            if (GamebaseUnitySDK.IsInitialized == false)
            {
                if (callback != null)
                {
                    callback(null, new GamebaseError(GamebaseErrorCode.NOT_INITIALIZED, Domain));
                }
                return;
            }

            var requestVo = TermsMessage.GetQueryTermsMessage();
            WebSocket.Instance.Request(requestVo, (response, error) =>
            {
                if (error != null)
                {
                    if (callback != null)
                    {
                        callback(null, error);
                    }   
                    return;
                }

                var vo = JsonMapper.ToObject<TermsResponse.QueryTerms>(response);
                GamebaseResponse.Terms.QueryTermsResult queryTermsResult = null;

                if (vo.header.isSuccessful == true)
                {
                    queryTermsResult = JsonMapper.ToObject<GamebaseResponse.Terms.QueryTermsResult>(JsonMapper.ToJson(vo.terms));
                }
                else
                {
                    error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVo.transactionId, requestVo.apiId, vo.header, Domain);
                }

                if (callback != null)
                {
                    callback(queryTermsResult, error);
                }   
            });
        }
        
        public bool IsShowingTermsView()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return false;
        }
    }
}
#endif
