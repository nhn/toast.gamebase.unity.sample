using UnityEngine;

namespace Toast.Cef.Webview
{
    [ExecuteInEditMode]
    public class CefEditorCleaner : MonoBehaviour
    {

#if UNITY_EDITOR
        private const string CLIEANER_NAME = "CefEditorCleaner";

        private bool isClean = false;

        public static void Create()
        {
            GameObject gameObj = GameObject.Find(CLIEANER_NAME);
            if (null != gameObj)
            {
                CefEditorCleaner cleaner = gameObj.GetComponent<CefEditorCleaner>();
                if (null != cleaner)
                {
                    cleaner.isClean = true;
                }

                DestroyImmediate(gameObj);
            }

            gameObj             = new GameObject(CLIEANER_NAME, typeof(CefEditorCleaner));
            gameObj.hideFlags   = HideFlags.HideAndDontSave;

#if UNITY_2017_2_OR_NEWER
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
            UnityEditor.EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
#else
            UnityEditor.EditorApplication.playmodeStateChanged -= Create;
            UnityEditor.EditorApplication.playmodeStateChanged += Create;
#endif
        }

#if UNITY_2017_2_OR_NEWER
        public static void OnPlaymodeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            Create();
        }
#endif

        private void OnDestroy()
        {
            if (isClean == true)
            {
                return;
            }
            
            CefManager.UnloadWeb(true);
        }
#endif
    }
}
