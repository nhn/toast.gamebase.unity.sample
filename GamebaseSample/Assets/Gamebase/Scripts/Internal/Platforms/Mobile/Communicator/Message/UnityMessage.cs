namespace Toast.Gamebase.Internal.Mobile
{
    public class UnityMessage
    {
        private const string RESPONSE_METHOD_NAME = "OnAsyncEvent";

        public string scheme                = string.Empty;
        public int handle                   = -1;  
        public string jsonData              = string.Empty;
        public string extraData             = string.Empty;
        public string gameObjectName        = string.Empty;
        public string responseMethodName    = string.Empty;

        public UnityMessage(string scheme, int handle = -1, string jsonData = "", string extraData = "")
        {
            this.scheme = scheme;
            this.handle = handle;

            if (null != jsonData)
            {
                this.jsonData = jsonData;
            }

            if (null != extraData)
            {
                this.extraData = extraData;
            }
                        
            responseMethodName = RESPONSE_METHOD_NAME;
            gameObjectName = GamebaseGameObjectManager.GameObjectType.PLUGIN_TYPE.ToString();
        }
    }
}