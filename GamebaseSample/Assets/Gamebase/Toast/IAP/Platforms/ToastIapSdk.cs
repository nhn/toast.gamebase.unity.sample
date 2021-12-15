using UnityEngine;

namespace Toast.Internal
{
    public class ToastIapSdk : MonoBehaviour
    {
        private static string SERVICE_NAME = "iap";

        private static ToastIapSdk _instance;
        private IToastNativeIAP _nativeIAP = null;

        public static ToastIapSdk Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(ToastIapSdk)) as ToastIapSdk;
                    if (!_instance)
                    {
                        var container = GameObject.Find(Constants.SdkPluginObjectName);
                        if (container == null)
                        {
                            container = new GameObject(Constants.SdkPluginObjectName);
                        }

                        _instance = container.AddComponent<ToastIapSdk>();
                        DontDestroyOnLoad(_instance);

                        _instance.Initialize();
                    }
                }

                return _instance;
            }
        }

        private void Initialize()
        {
            //#if UNITY_EDITOR
            //            _nativeIAP = new ToastStandaloneLogger();
            //#elif UNITY_STANDALONE
            //            _nativeIAP = new ToastStandaloneLogger();
            //#elif UNITY_WEBGL
            //            _nativeIAP = new ToastWebGLLogger();
            //#endif
        }

        public IToastNativeIAP NativeIAP
        {
            get { return _nativeIAP; }
        }

        private static void SdkInitialize()
        {
            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "Initialize".ToLower()),
                new ToastIapInitializeAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "Products".ToLower()),
                new ToastIapProductDetailsAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "Purchases/Consumable".ToLower()),
                new ToastIapConsumablePurchasesAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "purchases/activated".ToLower()),
                new ToastIapActivatedPurchasesAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "purchases/restore".ToLower()),
                new ToastIapRestorePurchasesAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "Consume".ToLower()),
                new ToastIapConsumeAction());

            ToastActionHandler.RegisterAction(
                ToastUri.Create(SERVICE_NAME, "Purchase".ToLower()),
                new ToastIapPurchaseAction());

        }
    }
}

