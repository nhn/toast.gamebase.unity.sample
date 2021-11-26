namespace Toast.Gamebase.Internal
{
    public interface IGamebaseContact
    {
        void OpenContact(int handle);
        void OpenContact(GamebaseRequest.Contact.Configuration configuration, int handle);
        void RequestContactURL(int handle);
        void RequestContactURL(GamebaseRequest.Contact.Configuration configuration, int handle);
    }
}
