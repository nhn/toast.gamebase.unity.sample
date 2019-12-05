#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile
{
    public interface INativeMessageSender
    {
        string GetSync(string jsonString);

        void GetAsync(string jsonString);

        void Initialize(string className);
    }
}
#endif