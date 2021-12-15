namespace Toast.Internal
{
    public class NativeResponseHeader
    {
        public string TransactionId { get; set; }
    }

    public class NativeResponse
    {
        public string Uri { get; private set; }

        public NativeResponseHeader Header { get; private set; }

        public ToastResult Result { get; private set; }

        public JSONObject Body { get; private set; }

        private NativeResponse()
        {
        }

        public static NativeResponse FromJson(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString)) return null;
            var json = JsonUtils.SafeParse(jsonString);

            if (json == null || !ValidateFromJson(json)) return null;

            var response = new NativeResponse();
            var root = json.AsObject;
            var header = root[JsonKeys.Header].AsObject;
            var body = root[JsonKeys.Body].AsObject;

            response.Uri = root[JsonKeys.Uri];
            response.Header = new NativeResponseHeader { TransactionId = header[JsonKeys.TransactionId] };
            response.Result = new ToastResult(
                body[JsonKeys.IsSuccessful].AsBool,
                body[JsonKeys.ResultCode].AsInt,
                body[JsonKeys.ResultMessage].Value);
            response.Body = body;
            return response;
        }

        private static bool ValidateFromJson(JSONNode json)
        {
            var root = json.AsObject;
            // Root의 필수 키값 체크
            var isValidInRoot = root.ContainsKeys(JsonKeys.Uri, JsonKeys.Header, JsonKeys.Body);
            if (!isValidInRoot) return false;

            var header = root[JsonKeys.Header].AsObject;
            var body = root[JsonKeys.Body].AsObject;
            if (header == null || body == null) return false;

            // header와 body 에서 필수 키값 체크
            var isValidInnerValues = header.ContainsKey(JsonKeys.TransactionId) &&
                                     body.ContainsKeys(JsonKeys.IsSuccessful, JsonKeys.ResultCode,
                                         JsonKeys.ResultMessage);

            // body 필수 키값들의 타입 체크
            var isValidBodyTypes = body[JsonKeys.IsSuccessful].Tag == JSONNodeType.Boolean &&
                                   body[JsonKeys.ResultCode].Tag == JSONNodeType.Number
                                   && body[JsonKeys.ResultMessage].Tag == JSONNodeType.String;

            return isValidInnerValues && isValidBodyTypes;
        }

        public override string ToString()
        {
            return string.Format("Uri: {0}, Header: {1}, Result: {2}, Body: {3}", Uri, Header, Result, Body);
        }
    }
}