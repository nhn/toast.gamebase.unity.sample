namespace Toast.Internal
{
    public class ToastUnityMessage
    {
        private JSONObject _originJson;
        public string TransactionId { get; private set; }

        public ToastUnityMessage(string jsonString)
        {
            _originJson = JSONNode.Parse(jsonString).AsObject;

            JSONObject header = _originJson["header"].AsObject;

            if (header != null)
            {
                TransactionId = header["transactionId"];
            }
        }

        public string GetUri()
        {
            return _originJson["uri"];
        }

        public JSONObject GetPayload()
        {
            return _originJson["payload"].AsObject;
        }

        public override string ToString()
        {
            return _originJson.ToString();
        }
    }
}