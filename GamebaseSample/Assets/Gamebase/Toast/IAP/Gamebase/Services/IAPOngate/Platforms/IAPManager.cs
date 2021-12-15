using UnityEngine;

namespace Toast.Iap.Ongate
{

    public class IAPManager : MonoBehaviour, INativeManager
    {
        private static readonly string UNITY_GAME_OBJECT_NAME = "IAPManager";

        private static IAPManager _instance;
        private IAPService nativePlugin;

        public static IAPManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(IAPManager)) as IAPManager;
                    if (!_instance)
                    {
                        GameObject container = new GameObject();
                        container.name = UNITY_GAME_OBJECT_NAME;
                        _instance = container.AddComponent(typeof(IAPManager)) as IAPManager;
                        DontDestroyOnLoad(_instance);

                        _instance.Init();

                        DebugUtil.Log(UNITY_GAME_OBJECT_NAME + " Has been created.");
                    }
                }
                return _instance;
            }
        }

        public void Init()
        {
            nativePlugin = ScriptableObject.CreateInstance<StandaloneWebGLPlugin>();
        }

        public IAPService NativePlugin
        {
            get
            {
                return nativePlugin;
            }
        }

        public void OnNativeMessage(string jsonResult)
        {
            IAPCallbackHandler.Instance.HandleCallback(jsonResult);
        }
    }
}
