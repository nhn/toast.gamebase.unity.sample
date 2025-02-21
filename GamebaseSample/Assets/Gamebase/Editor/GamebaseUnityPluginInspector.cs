using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Toast.Gamebase.Internal;

namespace Toast.Gamebase.Inspector
{
#pragma warning disable 0618
    [CustomEditor(typeof(GamebaseUnitySDKSettings))]
#pragma warning restore 0618
    public class GamebaseUnityPluginInspector : Editor
    {
#pragma warning disable 0618
        GamebaseUnitySDKSettings settings = null;
#pragma warning restore 0618
        
        #region style
        private GUIStyle foldoutStyle = null;
        private GUIStyle versionStyle = null;
        private GUIStyle platformStyle = null;
        #endregion

        #region App Settings
        private bool appSettingsFoldout = true;
        private GUIContent appIdContent = null;
        private GUIContent appVersionContent = null;
        private GUIContent displayLanguageCodeContent = null;
        private GUIContent debugContent = null;
        private GUIContent useWebViewLoginContent = null;
        #endregion

        #region Platform
        private bool platformFoldout = true;
        private bool iosFoldout = true;
        private bool androidFoldout = true;
        private bool webglFoldout = true;
        private bool standaloneWindowsFoldout = true;
        private bool standaloneMacOsFoldout = true;
        private GUIContent storeContent = null;
        private GUIContent fcmContent = null;
        #endregion

        #region Popup
        private bool popupFoldout = true;
        private GUIContent popupContent = null;
        private GUIContent launchingPopupContent = null;
        private GUIContent banPopupContent = null;
        private GUIContent kickoutPopupContent = null;
        #endregion

        private void OnEnable()
        {
#pragma warning disable 0618
            settings = target as GamebaseUnitySDKSettings;
#pragma warning restore 0618

            CreateStyle();

            CreateContent();
        }

        private void CreateContent()
        {
            if (null == appIdContent)
            {
                appIdContent = new GUIContent("App ID", "Project ID registered in TOAST Cloud.");
            }

            if (null == appVersionContent)
            {
                appVersionContent = new GUIContent("App Version", "Client Version registered in TOAST Cloud.");
            }

            if (null == displayLanguageCodeContent)
            {
                displayLanguageCodeContent = new GUIContent("Display Language Code", "The language setting displayed in the Gamebase UI.");
            }

            if (null == debugContent)
            {
                debugContent = new GUIContent("Debug Mode", "Settings for debugging the Gamebase.");
            }

            if (null == useWebViewLoginContent)
            {
                useWebViewLoginContent = new GUIContent("Use WebView Login", "Settings for using webview on Gamebase.");
            }

            if (null == storeContent)
            {
                storeContent = new GUIContent("Store Code", "Store information required to initialize IAP(In-App Purchase).");
            }

            if (null == fcmContent)
            {
                fcmContent = new GUIContent("FCM Sender ID(Deprecated)", "Sender ID for Firebase Cloud Messaging (FCM) use.\nDeprecated As of release 2.6.0.\nRefer to the Android guide to configure the google-services.json file.");
            }

            if (null == popupContent)
            {
                popupContent = new GUIContent("Enable Popup", "This is the setting for whether to use the default pop-up provided by the Gamebase SDK.");
            }

            if (null == launchingPopupContent)
            {
                launchingPopupContent = new GUIContent("Enable Launching Status Popup", "This is a setting for whether to use the default pop-up provided by Gamebase SDK when the game can not be played.");
            }

            if (null == banPopupContent)
            {
                banPopupContent = new GUIContent("Enable Ban Popup", "This is a setting for whether to use the default pop-up provided by Gamebase SDK when the user is banned.");
            }

            if (null == kickoutPopupContent)
            {
                kickoutPopupContent = new GUIContent("Enable Kickout Popup", "This is a setting for whether to use the default pop-up provided by Gamebase SDK when the user is kickout.");
            }
        }

        private void CreateStyle()
        {
            if (null == foldoutStyle)
            {
                foldoutStyle = new GUIStyle(EditorStyles.foldout);
                foldoutStyle.fontSize = 12;
                foldoutStyle.fontStyle = FontStyle.Bold;
            }

            if (null == versionStyle)
            {
                versionStyle = new GUIStyle();
                versionStyle.normal.textColor = Color.cyan;
            }

            if (null == platformStyle)
            {
                platformStyle = new GUIStyle(EditorStyles.foldout);
                platformStyle.fontSize = 11;
            }
        }

        public override void OnInspectorGUI()
        {
            DrawAppSettings();

            DrawPlatformSettings();

            DrawPopupSettings();

            DrawSDKVersion();

            if (true == GUI.changed)
            {
                EditorUtility.SetDirty(target);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }

        private void DrawSDKVersion()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Gamebase Unity SDK Version : v" + GamebaseUnitySDK.SDKVersion, versionStyle);
        }

        private void DrawAppSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            appSettingsFoldout = EditorGUILayout.Foldout(appSettingsFoldout, "App Settings", foldoutStyle);

            if (appSettingsFoldout)
            {
                settings.appID = EditorGUILayout.TextField(appIdContent, settings.appID);
                settings.appVersion = EditorGUILayout.TextField(appVersionContent, settings.appVersion);
                settings.displayLanguageCode = EditorGUILayout.TextField(displayLanguageCodeContent, settings.displayLanguageCode);
                settings.isDebugMode = EditorGUILayout.BeginToggleGroup(debugContent, settings.isDebugMode);

                EditorGUILayout.EndToggleGroup();
            }
        }

        private void DrawPlatformSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            platformFoldout = EditorGUILayout.Foldout(platformFoldout, "Platform Settings", foldoutStyle);

            if (platformFoldout)
            {
                EditorGUI.indentLevel = 1;

                iosFoldout = EditorGUILayout.Foldout(iosFoldout, "iOS", platformStyle);

                if (iosFoldout)
                {
                    settings.storeCodeIOS = EditorGUILayout.TextField(storeContent, settings.storeCodeIOS);
                }

                EditorGUILayout.Space();

                androidFoldout = EditorGUILayout.Foldout(androidFoldout, "Android", platformStyle);

                if (androidFoldout)
                {
                    settings.storeCodeAndroid = EditorGUILayout.TextField(storeContent, settings.storeCodeAndroid);
                    settings.fcmSenderId = EditorGUILayout.TextField(fcmContent, settings.fcmSenderId);
                }

                EditorGUILayout.Space();

                webglFoldout = EditorGUILayout.Foldout(webglFoldout, "WebGL", platformStyle);

                if (webglFoldout)
                {
                    settings.storeCodeWebGL = EditorGUILayout.TextField(storeContent, settings.storeCodeWebGL);
                }

                EditorGUILayout.Space();

                standaloneWindowsFoldout = EditorGUILayout.Foldout(standaloneWindowsFoldout, "Windows", platformStyle);

                if (standaloneWindowsFoldout)
                {
                    settings.storeCodeStandaloneWindows = EditorGUILayout.TextField(storeContent, settings.storeCodeStandaloneWindows);
                    settings.useWebViewLogin = EditorGUILayout.BeginToggleGroup(useWebViewLoginContent, settings.useWebViewLogin);
                    EditorGUILayout.EndToggleGroup();
                }
                
                EditorGUILayout.Space();

                standaloneMacOsFoldout = EditorGUILayout.Foldout(standaloneMacOsFoldout, "macOS", platformStyle);

                if (standaloneMacOsFoldout)
                {
                    settings.storeCodeStandaloneMacOS = EditorGUILayout.TextField(storeContent, settings.storeCodeStandaloneMacOS);
                }
                
                EditorGUI.indentLevel = 0;
            }
        }

        private void DrawPopupSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            popupFoldout = EditorGUILayout.Foldout(popupFoldout, "Popup Settings", foldoutStyle);

            if (popupFoldout)
            {
                settings.enablePopup = EditorGUILayout.BeginToggleGroup(popupContent, settings.enablePopup);

                EditorGUI.indentLevel = 1;

                settings.enableLaunchingStatusPopup = EditorGUILayout.BeginToggleGroup(launchingPopupContent, settings.enableLaunchingStatusPopup);
                EditorGUILayout.EndToggleGroup();

                settings.enableBanPopup = EditorGUILayout.BeginToggleGroup(banPopupContent, settings.enableBanPopup);
                EditorGUILayout.EndToggleGroup();

                EditorGUI.indentLevel = 0;

                EditorGUILayout.EndToggleGroup();
            }
        }
    }
}