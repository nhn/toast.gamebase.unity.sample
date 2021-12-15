#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseTerms : IGamebaseTerms
    {
        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain))
                    return typeof(CommonGamebase).Name;

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public void ShowTermsView(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.DataContainer>>(handle));
        }

        public void UpdateTerms(GamebaseRequest.Terms.UpdateTermsConfiguration configuration, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle));
        }

        public void QueryTerms(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Terms.QueryTermsResult>>>(handle));
        }
    }
}
#endif
