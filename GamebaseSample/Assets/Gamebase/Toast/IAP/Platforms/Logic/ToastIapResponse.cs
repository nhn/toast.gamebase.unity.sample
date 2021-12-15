using Toast.Internal;

namespace Toast.Iap
{
    public class ToastIapResponse
    {
        private class HeaderKeys
        {
            internal const string isSuccessful = "isSuccessful";
            internal const string resultCode = "resultCode";
            internal const string resultMessage = "resultMessage";
        }

        public class Header
        {
            public bool _isSuccessful;
            public int _resultCode;
            public string _resultMessage;

            internal static Header From(JSONObject jsonObject)
            {
                var result = new Header
                {
                    _isSuccessful = jsonObject[HeaderKeys.isSuccessful],
                    _resultCode = jsonObject[HeaderKeys.resultCode],
                    _resultMessage = jsonObject[HeaderKeys.resultMessage]
                };
                return result;
            }
        }
    }
}