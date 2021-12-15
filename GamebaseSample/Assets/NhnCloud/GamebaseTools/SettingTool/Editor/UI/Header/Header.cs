using NhnCloud.GamebaseTools.SettingTool.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class Header
    {
        /// <summary> 
        /// title(SettingTool)과 Align을 맞추기 위해 높이 값을 지정한다.
        /// </summary>
        private const float HEIGHT_VERSION_LABEL = 35;

        private const int CONTENT_WIDTH = 210;

        private const string TEXT_TITLE_SETTING_TOOL = "Gamebase Setting Tool";
        private const string TEXT_TITLE_STATUS = "Setting Tool Status";
        private const string TEXT_TITLE_SDK_DOWNLOAD = "SDK Download";
        private const string TEXT_TITLE_LINK = "Link";

        private const string TEXT_LINK_SETTING_TOOL_GUIDE = "- SettingTool Guide";
        private const string TEXT_LINK_GAMEBASE_GUIDE = "- Gamebase Guide";
        private const string TEXT_LINK_GAMEBASE_SAMPLE_APP_GITHUB = "- Gamebase SampleApp GitHub";
        private const string TEXT_LINK_EDM4U_GITHUB = "- EDM4U GitHub";

        private Rect headerArea;
        private SettingToolCallback.VoidDelegate onClickGamebaseSdkDownload;
        private SettingToolCallback.VoidDelegate onClickNaverCefePlugDownload;

        private string settingToolUpdateStatus;
        private string gamebaseDownloadMessage;

        private Texture2D settingToolUpdateIcon;
        private Texture2D downloadIcon;
        private Texture2D linkIcon;
                
        public Header(Rect headerArea)
        {
            this.headerArea = headerArea;
        }

        public void Initialize(SettingToolCallback.VoidDelegate onClickGamebaseSdkDownload, SettingToolCallback.VoidDelegate onClickNaverCefePlugDownload)
        {
            this.onClickGamebaseSdkDownload = onClickGamebaseSdkDownload;
            this.onClickNaverCefePlugDownload = onClickNaverCefePlugDownload;
            SetSettingToolUpdateStatusInfo();

            downloadIcon = EditorGUIUtility.FindTexture(ToolStyles.BUILT_IN_RESOURCE_COLLAB_PULL);
            linkIcon = EditorGUIUtility.FindTexture(ToolStyles.BUILT_IN_RESOURCE_D_FAVORITE);

            CreateGamebaseDownloadMessage();
        }

        private void SetSettingToolUpdateStatusInfo()
        {
            settingToolUpdateStatus = DataManager.GetData<string>(DataKey.SETTING_TOOL_UPDATE_STATUS);

            switch (settingToolUpdateStatus)
            {
                case SettingToolUpdateStatus.MANDATORY:
                    {
                        settingToolUpdateIcon = EditorGUIUtility.FindTexture(ToolStyles.BUILT_IN_RESOURCE_CONSOLE_ERRORICON_SML);
                        break;
                    }
                case SettingToolUpdateStatus.OPTIONAL:
                    {
                        settingToolUpdateIcon = EditorGUIUtility.FindTexture(ToolStyles.BUILT_IN_RESOURCE_CONSOLE_WARNICON_SML);
                        break;
                    }
                default:
                    {
                        settingToolUpdateIcon = EditorGUIUtility.FindTexture(ToolStyles.BUILT_IN_RESOURCE_COLLAB_NEW);
                        break;
                    }
            }            
        }

        private void CreateGamebaseDownloadMessage()
        {
            var data = DataManager.GetData<SettingToolResponse.Version>(DataKey.VERSION);
            var message = new StringBuilder();
            message.AppendLine();
            message.AppendLine(string.Format("Download path:{0}", "{PROJECT_PATH}/GamebaseSDK"));
            message.AppendLine();
            message.AppendLine("Gamebase SDK for Unity");
            message.AppendLine(string.Format("  - Current version:{0}", GamebaseInfo.GetCurrentVersion(EditorPrefsKey.UNITY_CURRENT_VERSION)));
            message.AppendLine(string.Format("  - Newest version:{0}", data.unity.newest));
            message.AppendLine();
            message.AppendLine("Gamebase SDK for Android");
            message.AppendLine(string.Format("  - Current version:{0}", GamebaseInfo.GetCurrentVersion(EditorPrefsKey.ANDROID_CURRENT_VERSION)));
            message.AppendLine(string.Format("  - Newest version:{0}", data.android.newest));
            message.AppendLine();
            message.AppendLine("Gamebase SDK for iOS");
            message.AppendLine(string.Format("  - Current version:{0}", GamebaseInfo.GetCurrentVersion(EditorPrefsKey.IOS_CURRENT_VERSION)));
            message.AppendLine(string.Format("  - Newest version:{0}", data.ios.newest));

            gamebaseDownloadMessage = message.ToString();
        }

        private string GetNaverCafeDownloadMessage()
        {
            var message = new StringBuilder();
            message.AppendLine();
            message.AppendLine(Multilanguage.GetString("PLUG_SDK_SETTING_MESSAGE"));

            var androidManifest = string.Format("{0}/Plugins/Android/androidManifest.xml", Application.dataPath);

            if (File.Exists(androidManifest) == true)
            {
                message.AppendLine();
                message.AppendLine(Multilanguage.GetString("PLUG_SDK_ANDROIDMANIFEST_BACKUP_POPUP_MESSAGE"));

                message.AppendLine();
                message.AppendLine(string.Format("{0}/Plugins/Android/androidManifest_$[DateTime.Now].xml", Application.dataPath));
            }

            return message.ToString();
        }

        public void Draw()
        {
            GUILayout.BeginArea(headerArea, ToolStyles.padding_top_left_right_10);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        DrawVersion();

                        EditorGUILayout.BeginHorizontal(ToolStyles.padding_top_left_right_10);
                        {
                            DrawSettingToolDownload();
                            DrawSdkDownload();
                            DrawLink();
                            DrawAd();

                            EditorGUILayout.EndHorizontal();
                        }

                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.EndArea();
            }
        }

        private void DrawVersion()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label(TEXT_TITLE_SETTING_TOOL, ToolStyles.SettingToolName);
                GUILayout.Label(SettingTool.VERSION, ToolStyles.SettingToolVersion, GUILayout.Height(HEIGHT_VERSION_LABEL));

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawSettingToolDownload()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label(new GUIContent(settingToolUpdateIcon), ToolStyles.IconLabel);

                EditorGUILayout.BeginVertical();
                {
                    GUILayout.Label(TEXT_TITLE_STATUS, ToolStyles.TitleLabel, GUILayout.Width(CONTENT_WIDTH));

                    switch (settingToolUpdateStatus)
                    {
                        case SettingToolUpdateStatus.NONE:
                            {
                                GUILayout.Label(Multilanguage.GetString("UI_TEXT_NOT_UPDATE"), ToolStyles.DefaultLabel);
                                GUI.enabled = false;
                                break;
                            }
                        case SettingToolUpdateStatus.MANDATORY:
                            {
                                GUILayout.Label(Multilanguage.GetString("UI_TEXT_MANDATORY_UPDATE"), ToolStyles.DefaultLabelRed);
                                break;
                            }
                        case SettingToolUpdateStatus.OPTIONAL:
                            {
                                GUILayout.Label(Multilanguage.GetString("UI_TEXT_OPTIONAL_UPDATE"), ToolStyles.DefaultLabelGreen);
                                break;
                            }
                    }

                    if (GUILayout.Button(Multilanguage.GetString("UI_TEXT_DOWNLOAD_SETTINGTOOL"), GUILayout.Width(150), GUILayout.Height(30)) == true)
                    {
                        Application.OpenURL("https://docs.toast.com/ko/Download/#game-gamebase");
                    }

                    GUI.enabled = true;

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawSdkDownload()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label(new GUIContent(downloadIcon), ToolStyles.IconLabel);

                EditorGUILayout.BeginVertical();
                {
                    GUILayout.Label(TEXT_TITLE_SDK_DOWNLOAD, ToolStyles.TitleLabel, GUILayout.Width(CONTENT_WIDTH));

                    switch (DataManager.GetData<string>(DataKey.GAMEBASE_UPDATE_STATUS))
                    {
                        case GamebaseUpdateStatus.NONE:
                            {
                                GUILayout.Label(Multilanguage.GetString("UI_TEXT_NOT_UPDATE"), ToolStyles.DefaultLabel);
                                GUI.enabled = false;
                                break;
                            }
                        case GamebaseUpdateStatus.DOWNLOAD_REQUIRED:
                            {
                                GUILayout.Label(Multilanguage.GetString("UI_TEXT_MANDATORY_UPDATE"), ToolStyles.DefaultLabelRed);
                                break;
                            }
                        case GamebaseUpdateStatus.UPDATE:
                            {
                                GUILayout.Label(Multilanguage.GetString("UI_TEXT_OPTIONAL_UPDATE"), ToolStyles.DefaultLabelGreen);
                                break;
                            }
                    }

                    if (GUILayout.Button("Gamebase SDK", GUILayout.Width(120), GUILayout.Height(30)) == true)
                    {
                        if (EditorUtility.DisplayDialog(
                            Multilanguage.GetString("POPUP_DOWNLOAD_SDK_TITLE"),
                            gamebaseDownloadMessage,
                            Multilanguage.GetString("POPUP_OK"),
                            Multilanguage.GetString("POPUP_CANCEL")) == true)
                        {
                            if (EditorUtility.DisplayDialog(
                                Multilanguage.GetString("POPUP_DOWNLOAD_SDK_TITLE"),
                                Multilanguage.GetString("POPUP_006_MESSAGE"),
                                Multilanguage.GetString("POPUP_OK"),
                                Multilanguage.GetString("POPUP_CANCEL")) == true)
                            {
                                if (onClickGamebaseSdkDownload != null)
                                {
                                    onClickGamebaseSdkDownload();
                                }
                            }
                        }
                    }
                    GUI.enabled = true;

                    GUILayout.Space(5);

                    if (GUILayout.Button(Multilanguage.GetString("PLUG_SDK_SETTING_BUTTON"), GUILayout.Width(120), GUILayout.Height(30)) == true)
                    {
                        if (onClickNaverCefePlugDownload != null)
                        {
                            if (EditorUtility.DisplayDialog(
                                Multilanguage.GetString("PLUG_SDK_SETTING_INFOMATION_TITLE"),
                                GetNaverCafeDownloadMessage(),
                                Multilanguage.GetString("POPUP_OK"),
                                Multilanguage.GetString("POPUP_CANCEL")) == true)
                            {
                                onClickNaverCefePlugDownload();
                            }

                        }
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawLink()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label(new GUIContent(linkIcon), ToolStyles.IconLabel);

                EditorGUILayout.BeginVertical();
                {
                    GUILayout.Label(TEXT_TITLE_LINK, ToolStyles.TitleLabel, GUILayout.Width(CONTENT_WIDTH));

                    DrawSampleAppLink();

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawSampleAppLink()
        {
            if (GUILayout.Button(TEXT_LINK_SETTING_TOOL_GUIDE, ToolStyles.LinkButton, GUILayout.Width(180)) == true)
            {
                Application.OpenURL("https://docs.toast.com/en/Game/Gamebase/en/unity-started/#using-the-setting-tool");
            }

            if (GUILayout.Button(TEXT_LINK_GAMEBASE_GUIDE, ToolStyles.LinkButton, GUILayout.Width(180)) == true)
            {
                Application.OpenURL("https://docs.toast.com/en/Game/Gamebase/en/unity-started/");
            }

            if (GUILayout.Button(TEXT_LINK_GAMEBASE_SAMPLE_APP_GITHUB, ToolStyles.LinkButton, GUILayout.Width(180)) == true)
            {
                Application.OpenURL("https://github.com/nhn/toast.gamebase.unity.sample");
            }

            if (GUILayout.Button(TEXT_LINK_EDM4U_GITHUB, ToolStyles.LinkButton, GUILayout.Width(180)) == true)
            {
                Application.OpenURL("https://github.com/googlesamples/unity-jar-resolver");
            }
        }

        private void DrawAd()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Advertisement", ToolStyles.TitleLabel, GUILayout.Width(CONTENT_WIDTH));

                EditorGUILayout.EndHorizontal();
            }            
        }
    }
}