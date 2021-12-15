using Toast.Logger;
using UnityEngine;

namespace Toast.Internal
{
    public class ToastLoggerSdk : MonoBehaviour
    {
        private static string SERVICE_NAME = "logger";

        private static ToastLoggerSdk _instance;
        private IToastNativeLogger _nativeLogger = null;

        public static ToastLoggerSdk Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(ToastLoggerSdk)) as ToastLoggerSdk;
                    if (!_instance)
                    {
                        var container = GameObject.Find(Constants.SdkPluginObjectName);
                        if (container == null)
                        {
                            container = new GameObject(Constants.SdkPluginObjectName);
                        }

                        _instance = container.AddComponent<ToastLoggerSdk>();
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
            _nativeLogger = new ToastStandaloneLogger();
#elif UNITY_STANDALONE
            _nativeLogger = new ToastStandaloneLogger();
#elif UNITY_WEBGL
            _nativeLogger = new ToastWebGLLogger();
#endif
        }

        public IToastNativeLogger NativeLogger
        {
            get { return _nativeLogger; }
        }

        private static void SdkInitialize()
        {
            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "initialize".ToLower()),
                new ToastLoggerInitializeAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "Log".ToLower()),
                new ToastLoggerLogAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "SetUserField".ToLower()),
                new ToastLoggerSetUserFieldAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "Exception".ToLower()),
                new ToastLoggerReportAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "SetLoggerListener".ToLower()),
                new ToastLoggerSetLoggerListenerAction());

        }
    }
}

