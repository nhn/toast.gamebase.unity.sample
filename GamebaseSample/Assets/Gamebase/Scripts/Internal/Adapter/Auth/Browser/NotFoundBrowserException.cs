using System;
using System.Runtime.Serialization;
using Toast.Gamebase.Internal;

namespace Toast.Gamebase.Internal.Auth.Browser
{
    [Serializable]
    public class NotFoundBrowserException : Exception
    {
        public NotFoundBrowserException()
        : base(GamebaseStrings.AUTH_IDP_LOGIN_SUPPORTED_BROWSER_NOT_FOUND) { }

        public NotFoundBrowserException(string message)
            : base(message) { }

        public NotFoundBrowserException(string message, Exception innerException)
            : base(message, innerException) { }

        protected NotFoundBrowserException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
