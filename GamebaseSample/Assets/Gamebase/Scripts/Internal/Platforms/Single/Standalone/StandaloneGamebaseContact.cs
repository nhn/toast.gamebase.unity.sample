#if UNITY_EDITOR || UNITY_STANDALONE
namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseContact : CommonGamebaseContact
    {
        public StandaloneGamebaseContact()
        {
            Domain = typeof(StandaloneGamebaseContact).Name;
        }

        public override void OpenContact(int handle)
        {
            GamebaseContact.Instance.OpenContact(handle);
        }

        public override void OpenContact(GamebaseRequest.Contact.Configuration configuration, int handle)
        {
            GamebaseContact.Instance.OpenContact(configuration, handle);
        }
    }
}
#endif