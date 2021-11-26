using System.Collections;
using UnityEditor;

namespace NhnCloud.GamebaseTools.SettingTool.Util
{
    public class EditorCoroutine
    {
        private readonly IEnumerator routine;

        public static EditorCoroutine Start(IEnumerator enumerator)
        {
            EditorCoroutine coroutine = new EditorCoroutine(enumerator);
            coroutine.Start();
            return coroutine;
        }

        public EditorCoroutine(IEnumerator enumerator)
        {
            routine = enumerator;
        }

        private void Start()
        {
#if UNITY_EDITOR
            EditorApplication.update += Update;
#endif
        }

        public void Stop()
        {
#if UNITY_EDITOR
            EditorApplication.update -= Update;
#endif
        }

        private void Update()
        {
            if (routine.MoveNext() == false)
            {
                Stop();
            }
        }
    }
}