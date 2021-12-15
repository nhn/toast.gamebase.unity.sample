using UnityEngine;

namespace Toast.Internal
{
    public class ToastNativePlugin : MonoBehaviour
    {
        private static ToastNativePlugin _instance;
        private IToastPlatformMessenger _nativePlugin = null;

        public static ToastNativePlugin Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(ToastNativePlugin)) as ToastNativePlugin;
                    if (!_instance)
                    {
                        var container = GameObject.Find(Constants.SdkPluginObjectName);
                        if (container == null)
                        {
                            container = new GameObject(Constants.SdkPluginObjectName);
                        }

                        _instance = container.AddComponent<ToastNativePlugin>();
                        DontDestroyOnLoad(_instance);

                        _instance.Initialize();
                    }
                }

                return _instance;
            }
        }

        private void Initialize()
        {
#if UNITY_EDITOR
            _nativePlugin = new ToastStandaloneWebGLMessenger();
#elif UNITY_IOS
            _nativePlugin = new ToastIosMessenger();
#elif UNITY_ANDROID
            _nativePlugin = new ToastAndroidMessenger();
#elif UNITY_STANDALONE
            _nativePlugin = new ToastStandaloneWebGLMessenger();
#elif UNITY_WEBGL
            _nativePlugin = new ToastStandaloneWebGLMessenger();
#else
            _nativePlugin = new ToastStubMessenger();
#endif

            if (!Dispatcher.IsInitialize())
            {
                Dispatcher.Initialize();
            }
        }

        public IToastPlatformMessenger NativePlugin
        {
            get { return _nativePlugin; }
        }

        /// <summary>
        /// Receive callback data from native library
        /// </summary>
        // TODO Not Implementation method
        public void ReceiveFromNative(string message)
        {
            ToastLog.Debug("Receive nativeMethod : " + message);

            var response = NativeResponse.FromJson(message);

            var callbackId = response.Header.TransactionId;
            var callback = ToastCallbackManager.Instance[callbackId];
            if (callback != null)
            {
                try
                {
                    string uri = response.Uri;

                    var result = new ToastResult(
                        response.Result.IsSuccessful,
                        response.Result.Code,
                        response.Result.Message);

                    callback(result, response);
                }
                finally
                {
                    ToastCallbackManager.Instance.RemoveCallback(callbackId);
                }
            }
        }
    }
}