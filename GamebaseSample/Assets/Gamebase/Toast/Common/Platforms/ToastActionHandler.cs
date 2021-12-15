using System.Collections.Generic;

namespace Toast.Internal
{
    public class ToastActionHandler
    {
        private const string INSTANCE_LOGGER = "toast://logger/";

        private static Dictionary<string, ToastUnityAction> _actions = new Dictionary<string, ToastUnityAction>();

        public static void RegisterAction(string uri, ToastUnityAction action)
        {
            _actions.Add(uri, action);
        }

        public static ToastUnityAction GetAction(string uri)
        {
            if (_actions.ContainsKey(uri) == false)
            {
                if (uri.Contains(INSTANCE_LOGGER) == true)
                {
                    string AppKey = uri.Substring(INSTANCE_LOGGER.Length);
                    string[] words = AppKey.Split('/');
                    ToastInstanceLoggerSdk.Instance.NativeInstanceLogger.SetAppKey(words[0]);
                    uri = "toast://instancelogger/" + words[1];

                    if (_actions.ContainsKey(uri) == false)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            return _actions[uri];
        }
    }
}
