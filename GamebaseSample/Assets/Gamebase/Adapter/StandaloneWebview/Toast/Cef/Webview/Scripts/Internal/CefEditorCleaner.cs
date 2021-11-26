using Toast.Cef.Webview.Internal;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Toast.Cef.Webview
{
    [ExecuteInEditMode]
    public class CefCleaner : MonoBehaviour
    {
        private const string CLIEANER_NAME = "CefCleaner";

        private bool isClean = false;

        public static void Create()
        {
            GameObject gameObj = GameObject.Find(CLIEANER_NAME);

            if (gameObj != null)
            {
                CefCleaner cleaner = gameObj.GetComponent<CefCleaner>();
                if (cleaner != null)
                {
                    cleaner.isClean = true;
                }

                DestroyImmediate(gameObj);
            }

            gameObj = new GameObject(CLIEANER_NAME, typeof(CefCleaner))
            {
                hideFlags = HideFlags.HideAndDontSave
            };

#if UNITY_EDITOR
#if UNITY_2017_2_OR_NEWER
            EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
#else
            EditorApplication.playmodeStateChanged -= Create;
            EditorApplication.playmodeStateChanged += Create;
#endif
#endif
        }

#if UNITY_EDITOR
#if UNITY_2017_2_OR_NEWER
        public static void OnPlaymodeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            Create();
        }
#endif
#endif

        private void OnDestroy()
        {
            if (isClean == true)
            {
                return;
            }

            NativeMethods.ExitCef();
        }
    }
}
