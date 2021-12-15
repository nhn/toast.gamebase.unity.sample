using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    internal interface IGamebaseTerms
    {
        void ShowTermsView(int handle);
        void UpdateTerms(GamebaseRequest.Terms.UpdateTermsConfiguration configuration, int handle);
        void QueryTerms(int handle);
    }
}
