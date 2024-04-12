using NhnCloud.GamebaseTools.SettingTool.Data;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class SettingToolUi : IDisposable
    {
        private Header header;
        private SdkSetting sdkSetting;
        private Copyright copyright;

        private Rect settingToolArea;
        private Rect headerArea;
        private Rect copyrightArea;
        private Rect sdkSettingArea;

        private bool isInitialized;

        private string downloadFileName;
        private float progress;

        public SettingToolUi()
        {
            //------------------------------
            // settingTool
            //------------------------------
            settingToolArea = new Rect(0, 0, 1024, 768);

            //------------------------------
            // header
            //------------------------------
            headerArea = new Rect(0, 0, 1024, 230);

            //------------------------------
            // copyright
            //------------------------------
            var copyrightHeight = 24;
            copyrightArea = new Rect(0, settingToolArea.height - copyrightHeight, settingToolArea.width, copyrightHeight);
            copyright = new Copyright(copyrightArea);
            //------------------------------
            // SdkSetting
            //------------------------------
            sdkSettingArea = new Rect(0, headerArea.height, 1024, settingToolArea.height - headerArea.height - copyrightArea.height);
        }

        public void Dispose()
        {

        }

        public void Initialize(
            SettingToolCallback.VoidDelegate onClickGamebaseSdkDownload,
            SettingToolCallback.VoidDelegate onClickNaverCefePlugDownload,
            SettingToolCallback.VoidDelegate onClickSetting,
            SettingToolCallback.VoidDelegate onClickRemove)
        {
            header = new Header(headerArea);            
            header.Initialize(onClickGamebaseSdkDownload, onClickNaverCefePlugDownload);

            sdkSetting = new SdkSetting(sdkSettingArea);
            sdkSetting.Initialize(onClickSetting, onClickRemove);
            
            isInitialized = true;
        }

        public void RemoveSetting()
        {
            sdkSetting.RemoveSettings();
        }

        /// <summary>
        /// Called from OnGUI.
        /// </summary>
        public void Draw()
        {
            if (isInitialized == false)
            {
                return;
            }

            GUILayout.BeginArea(settingToolArea);
            {
                DrawProgress();

                EditorGUILayout.BeginVertical();
                {
                    header.Draw();

                    if (sdkSetting != null && DataManager.GetData<bool>(DataKey.HAS_GAMEBASE_SDK) == true)
                    {
                        sdkSetting.Draw();
                    }

                    copyright.Draw();

                    EditorGUILayout.EndVertical();
                }

                GUILayout.EndArea();
            }
        }

        public void ShowProgressBar(string downloadFileName, float progress)
        {
            this.downloadFileName = downloadFileName;
            this.progress = progress;

            if (string.IsNullOrEmpty(downloadFileName) == true || progress == 0)
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void DrawProgress()
        {
            if (string.IsNullOrEmpty(downloadFileName) == true || progress == 0)
            {
                return;
            }

            EditorUtility.DisplayProgressBar(
                Multilanguage.GetString("UI_TEXT_DOWNLOADING"),
                string.Format(
                    "{0}:{1}",
                    Multilanguage.GetString("UI_TEXT_DOWNLOAD_FILE"),
                    Path.GetFileName(downloadFileName)),
                progress);
        }
    }
}