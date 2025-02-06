using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    internal interface IGamebaseTerms
    {
        void ShowTermsView(GamebaseRequest.Terms.GamebaseTermsConfiguration configuration, int handle);
        void UpdateTerms(GamebaseRequest.Terms.UpdateTermsConfiguration configuration, int handle);
        void QueryTerms(int handle);
        bool IsShowingTermsView();
    }
}
