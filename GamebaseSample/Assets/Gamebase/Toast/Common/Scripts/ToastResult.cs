using System.Collections.Generic;
using Toast.Internal;

namespace Toast
{
    public class ToastResult
    {
        public bool IsSuccessful { get; private set; }

        public int Code { get; private set; }

        public string Message { get; private set; }

        public ToastResult(bool isSuccessful, int code, string message)
        {
            IsSuccessful = isSuccessful;
            Code = code;
            Message = message;
        }

        public override string ToString()
        {
            var json = JSONObject.FromDictionary(new Dictionary<string, object>
            {
                {"isSuccessful", IsSuccessful},
                {"code", Code},
                {"message", Message},
            });

            return json.ToString(2);
        }
    }
}