using UnityEditor;
using UnityEngine;

namespace Toast.Gamebase.Menu
{
    public static class GamebaseEditorManager
    {
        [MenuItem("Gamebase/Documentation")]
        public static void OpenGuidePage()
        {
            Application.OpenURL("https://docs.toast.com/ko/Game/Gamebase/ko/unity-started/");
        }

        [MenuItem("Gamebase/Release Note")]
        public static void OpenReleaseNotePage()
        {
            Application.OpenURL("https://docs.toast.com/ko/Game/Gamebase/ko/release-notes/");
        }

        [MenuItem("Gamebase/Download")]
        public static void OpenDownloadPage()
        {
            Application.OpenURL("https://docs.toast.com/ko/Download/#game-gamebase");
        }
    }
}