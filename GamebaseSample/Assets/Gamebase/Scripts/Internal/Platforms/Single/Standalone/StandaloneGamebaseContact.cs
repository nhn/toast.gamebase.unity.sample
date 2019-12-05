#if UNITY_EDITOR || UNITY_STANDALONE
namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseContact : CommonGamebaseContact
    {
        public StandaloneGamebaseContact()
        {
            Domain = typeof(StandaloneGamebaseContact).Name;
        }
    }
}
#endif