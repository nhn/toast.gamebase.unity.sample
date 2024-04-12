#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public static class ToolStyles
    {
        #region BuiltInResources
        /// <summary>
        /// SettingToolUpdateStatus.NONE
        /// </summary>
        public const string BUILT_IN_RESOURCE_COLLAB_NEW = "CollabNew";

        /// <summary>
        /// SettingToolUpdateStatus.MANDATORY
        /// </summary>
        public const string BUILT_IN_RESOURCE_CONSOLE_ERRORICON_SML = "console.erroricon.sml";

        /// <summary>
        /// SettingToolUpdateStatus.OPTIONAL
        /// </summary>
        public const string BUILT_IN_RESOURCE_CONSOLE_WARNICON_SML = "console.warnicon.sml";

        /// <summary>
        /// SDK Download
        /// </summary>
        public const string BUILT_IN_RESOURCE_COLLAB_PULL = "CollabPull";

        /// <summary>
        /// Link
        /// </summary>
        public const string BUILT_IN_RESOURCE_D_FAVORITE = "d_Favorite";
        #endregion

        public static readonly GUIStyle padding_top_left_right_10;
        public static readonly GUIStyle padding_top_left_right_20;
        public static readonly GUIStyle padding_left_right_20;
        public static readonly GUIStyle SettingToolName;
        public static readonly GUIStyle SettingToolVersion;
        public static readonly GUIStyle IconLabel;
        public static readonly GUIStyle CopyrightLabel;
        public static readonly GUIStyle LanguagePopup;
        public static readonly GUIStyle TitleLabel;
        public static readonly GUIStyle DefaultLabel;
        public static readonly GUIStyle DefaultLabelGreen;
        public static readonly GUIStyle DefaultLabelRed;
        public static readonly GUIStyle SmallLabel;
        public static readonly GUIStyle LinkButton;
        public static readonly GUIStyle AdapterCategory;
        public static readonly GUIStyle CheckBox;

        static ToolStyles()
        {
            padding_top_left_right_10 = new GUIStyle
            {
                padding = new RectOffset(10, 10, 10, 0)
            };

            padding_top_left_right_20 = new GUIStyle
            {
                padding = new RectOffset(20, 20, 20, 0)
            };

            padding_left_right_20 = new GUIStyle
            {
                padding = new RectOffset(20, 20, 0, 0)
            };

            SettingToolName = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                stretchWidth = false,
                fontSize = 30
            };

            SettingToolVersion = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.LowerLeft,
                fontSize = 12
            };

            IconLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(4, 4, 4, 4),
                stretchWidth = false,
                stretchHeight = false
            };

            CopyrightLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 10
            };

            LanguagePopup = new GUIStyle(EditorStyles.popup)
            {
                fontSize = 11
            };

            TitleLabel = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                //stretchWidth = false
            };

            DefaultLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12
            };

            GUIStyleState GreenDefaultLabelStyleState = new GUIStyleState
            {
                textColor = Color.green
            };

            DefaultLabelGreen = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12,
                normal = GreenDefaultLabelStyleState
            };

            GUIStyleState RedDefaultLabelStyleState = new GUIStyleState
            {
                textColor = Color.red
            };

            DefaultLabelRed = new GUIStyle(GUI.skin.label)
             {
                 alignment = TextAnchor.MiddleLeft,
                 fontSize = 12,
                 normal = RedDefaultLabelStyleState
            };

            GUIStyleState GrayDefaultLabelStyleState = new GUIStyleState
            {
                textColor = Color.gray
            };

            SmallLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 11,
                normal = GrayDefaultLabelStyleState
            };

            GUIStyleState LinkButtonStyleState = new GUIStyleState
            {
                background = null,
                textColor = new Color(17f / 255f, 164f / 255f, 251f / 255f, 1f)
            };

            LinkButton = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                normal = LinkButtonStyleState,
                hover = LinkButtonStyleState,
                active = LinkButtonStyleState,
            };

            AdapterCategory = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fixedWidth = 229
            };

            CheckBox = new GUIStyle(GUI.skin.toggle)
            {
                padding = new RectOffset(20, 0, 0, 0),
                fontSize = 13,
                fontStyle = FontStyle.Bold,
            };
        }
    }
}

#endif