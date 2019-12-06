using UnityEngine;
using UnityEditor;

namespace Toast.SmartDownloader.Infomation
{
    internal class SmartDlAboutWindow : EditorWindow
    {
        private const string MENU_OPEN_ABOUT = "Tools/TOAST/Smart Downloader/About";

        private const string TEXT_TITLE = "About Smart Downloader";
        private const string TEXT_PRODUCT = "Smart Downloader";
        private const string TEXT_VERSION = "Version";
        private const string TEXT_DOCUMENT = "Document";
        private const string TEXT_DOWNLOAD = "Download Unity Tool";
        private const string TEXT_COPYRIGHT = "ⓒ NHN Corp. All rights reserved.";

        private const string URL_DOCUMENT = "https://docs.toast.com/en/Game/Smart%20Downloader/en/Overview/";
        private const string URL_DOWNLOAD = "https://docs.toast.com/en/Download/#game-smart-downloader";

        private static SmartDlAboutWindow window;
        private static readonly Rect windowRect = new Rect(100, 100, 400, 250);


        [MenuItem(MENU_OPEN_ABOUT)]
        private static void Open()
        {
            if (window != null)
            {
                window.Close();
                window = null;
            }

            window = GetWindowWithRect<SmartDlAboutWindow>(windowRect, true, TEXT_TITLE);
        }


        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 380, 40));
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Label(TEXT_PRODUCT, Styles.ProductLabel);
            }
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(10, 54, 380, 180));
            EditorGUILayout.BeginVertical();
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label(TEXT_VERSION, GUILayout.ExpandWidth(false));
                    GUILayout.Label(SdkVersion.Version);
                }

                GUILayout.Space(4);

                EditorGUILayout.BeginVertical("box", GUILayout.Width(378), GUILayout.Height(144));
                {
                    if (GUILayout.Button(TEXT_DOCUMENT, Styles.LinkLabel) == true)
                    {
                        Application.OpenURL(URL_DOCUMENT);
                    }

                    if (GUILayout.Button(TEXT_DOWNLOAD, Styles.LinkLabel) == true)
                    {
                        Application.OpenURL(URL_DOWNLOAD);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(0, position.height - 18, position.width, 18));
            EditorGUILayout.BeginVertical(Styles.CopyrightBox);
            {
                GUILayout.Label(TEXT_COPYRIGHT, Styles.CopyrightLabel);
            }
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private static class Styles
        {
            public static readonly GUIStyle CopyrightBox;
            public static readonly GUIStyle CopyrightLabel;
            public static readonly GUIStyle ProductLabel;
            public static readonly GUIStyle LinkLabel;

            static Styles()
            {
                CopyrightBox = new GUIStyle(GUI.skin.FindStyle("ProgressBarBack"))
                {
                    name = "SmartDl_CopyrightBox",
                    alignment = TextAnchor.LowerCenter,
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, 0, 0),
                    fixedHeight = 18,
                    stretchHeight = true,
                    stretchWidth = true
                };

                CopyrightLabel = new GUIStyle(GUI.skin.label)
                {
                    name = "SmartDl_CopyrightLabel",
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 10,
                    normal = { textColor = Color.gray }
                };

                ProductLabel = new GUIStyle(GUI.skin.label)
                {
                    name = "SmartDl_ProductLabel",
                    fontSize = 28, 
                    fontStyle = FontStyle.Bold
                };

                LinkLabel = new GUIStyle(GUI.skin.label)
                {
                    name = "ToastKitLinkLabel",
                    richText = true,
                    stretchWidth = false,
                    normal = { textColor = new Color(0.31f, 0.5f, 0.972f) }
                };
            }
        }
    }
}