#if UNITY_EDITOR || UNITY_STANDALONE

using System.Collections.Generic;
using System.Text;
using Toast.Gamebase.Internal.Single.Communicator;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebasePurchase : CommonGamebasePurchase
    {
        public StandaloneGamebasePurchase()
        {
            Domain = typeof(StandaloneGamebasePurchase).Name;
        }
    }
}
#endif