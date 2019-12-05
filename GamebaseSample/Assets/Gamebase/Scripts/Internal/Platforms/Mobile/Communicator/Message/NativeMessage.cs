using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeMessage
    {
        public string scheme;
        public int handle;
        public string jsonData;
        public string extraData;
        public string gamebaseError;

        public GamebaseError GetGamebaseError()
        {
            if (string.IsNullOrEmpty(gamebaseError))
                return null;
            return JsonMapper.ToObject<GamebaseError>(gamebaseError);
        }
    }
}