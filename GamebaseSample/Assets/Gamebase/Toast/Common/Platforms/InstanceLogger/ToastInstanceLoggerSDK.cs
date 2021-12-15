using UnityEngine;

namespace Toast.Internal
{
    public class ToastInstanceLoggerSdk : MonoBehaviour
    {
        private static readonly string SERVICE_NAME = "Instancelogger";

        private static ToastInstanceLoggerSdk _instance;
        private IToastNativeInstanceLogger _nativeInstanceLogger = null;

        public static ToastInstanceLoggerSdk Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(ToastInstanceLoggerSdk)) as ToastInstanceLoggerSdk;
                    if (!_instance)
                    {
                        var container = GameObject.Find(Constants.SdkPluginObjectName);
                        if (container == null)
                        {
                            container = new GameObject(Constants.SdkPluginObjectName);
                        }

                        _instance = container.AddComponent<ToastInstanceLoggerSdk>();
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
            _nativeInstanceLogger = new ToastStandaloneInstanceLogger();
#elif UNITY_STANDALONE
            _nativeInstanceLogger = new ToastStandaloneInstanceLogger();
#elif UNITY_WEBGL
            _nativeInstanceLogger = new ToastWebGLInstanceLogger();
#endif
        }

        public IToastNativeInstanceLogger NativeInstanceLogger
        {
            get { return _nativeInstanceLogger; }
        }

        private static void SdkInitialize()
        {
            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "initialize".ToLower()),
                new ToastInstanceLoggerInitializeAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "Log".ToLower()),
                new ToastInstanceLoggerLogAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "SetUserField".ToLower()),
                new ToastInstanceLLoggerSetUserFiledAction());
        }
    }
}

