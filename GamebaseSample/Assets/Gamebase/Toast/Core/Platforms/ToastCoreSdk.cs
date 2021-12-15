using Toast.Core;
using UnityEngine;

namespace Toast.Internal
{
    public class ToastCoreSdk : MonoBehaviour
    {
        private static string SERVICE_NAME = "core";

        private static ToastCoreSdk _instance;
        private IToastNativeCore _nativeCore = null;

        public static ToastCoreSdk Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(ToastCoreSdk)) as ToastCoreSdk;
                    if (!_instance)
                    {
                        var container = GameObject.Find(Constants.SdkPluginObjectName);
                        if (container == null)
                        {
                            container = new GameObject(Constants.SdkPluginObjectName);
                        }

                        _instance = container.AddComponent<ToastCoreSdk>();
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
            _nativeCore = new ToastStandaloneCore();
#elif UNITY_STANDALONE
            _nativeCore = new ToastStandaloneCore();
#elif UNITY_WEBGL
            _nativeCore = new ToastWebGLCore();
#endif
        }

        public IToastNativeCore NativeCore
        {
            get { return _nativeCore; }
        }

        private static void SdkInitialize()
        {
            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "initialize".ToLower()),
                new ToastCoreInitializeAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "GetUserId".ToLower()),
                new ToastCoreGetUserIdAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "SetUserId".ToLower()),
                new ToastCoreSetUserIdAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "IsDebugMode".ToLower()),
                new ToastCoreIsDebugModeAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "SetDebugMode".ToLower()),
                new ToastCoreSetDebugModeAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "SetOptionalPolicies".ToLower()),
                new ToastCoreSetOptionalPolicies());

        }
    }
}

