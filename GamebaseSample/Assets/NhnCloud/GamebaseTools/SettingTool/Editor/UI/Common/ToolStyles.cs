#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public static class ToolStyles
    {
        public static GUISkin tookSkin;

        public static Texture2D icon_android;
        public static Texture2D icon_ios;
        public static Texture2D icon_windows;
        public static Texture2D icon_mac;
        public static Texture2D icon_download;
        public static Texture2D icon_setting;
        public static Texture2D icon_empty;
        public static Texture2D icon_check;
        public static Texture2D icon_remove;
        public static Texture2D icon_back;
        public static Texture2D icon_next;
        
        public static GUIStyle padding_intent_12;
        public static GUIStyle padding_intent_24;
        public static GUIStyle padding_1;
        public static GUIStyle padding_2;
        public static GUIStyle padding_4;
        public static GUIStyle padding_top_left_right_4;
        public static GUIStyle padding_top_left_right_8;
        
        public static GUIStyle padding_top_left_10;
        public static GUIStyle padding_top_left_right_10;
        public static GUIStyle padding_top_left_right_20;
        public static GUIStyle padding_left_right_10;
        public static GUIStyle padding_left_right_20;
        public static GUIStyle SettingToolName;
        public static GUIStyle WindowsName;
        public static GUIStyle SettingToolVersion;
        public static GUIStyle IconLabel;
        public static GUIStyle CopyrightLabel;
        public static GUIStyle LanguagePopup;
        public static GUIStyle TitleLabel;
        public static GUIStyle DefaultLabel;
        public static GUIStyle BoldLabel;
        public static GUIStyle PageLabel;
        public static GUIStyle DefaultLabelGreen;
        public static GUIStyle DefaultLabelYellow;
        public static GUIStyle DefaultLabelRed;
        public static GUIStyle SmallLabel;
        public static GUIStyle Button;
        public static GUIStyle SizeButton;
        public static GUIStyle LinkButton;

        public static GUIStyle AdapterCategory;
        public static GUIStyle CheckBox;
        public static GUIStyle CheckPartBox;
        public static GUIStyle CheckMustBox;
        public static GUIStyle CheckSelectedBox;
        public static GUIStyle CheckUpgradeBox;
        public static GUIStyle RadioBox;

        public static GUIStyle Box;
        public static GUIStyle MiniBox;
        
        public static GUIStyle Popup;

        public static GUIStyle TabButton;

        public static GUIStyle CheckLabel;
        public static GUIStyle XLabel;
        public static GUIStyle WarningLabel;


        static ToolStyles()
        {
            LoadStyle();
        }

        public static GUIContent GetPlatformContent(string platformName)
        {
            switch (platformName)
            {
                case SettingToolStrings.TEXT_ANDROID:
                    return new GUIContent(platformName, icon_android); 
                case SettingToolStrings.TEXT_IOS:
                    return new GUIContent(platformName, icon_ios);
                case SettingToolStrings.TEXT_WINDOWS:
                    return new GUIContent(platformName, icon_windows);
                case SettingToolStrings.TEXT_MACOS:
                    return new GUIContent(platformName, icon_mac);
            }
            return new GUIContent(platformName);
        }
        public static void LoadStyle()
        {
            tookSkin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/toolGUISkin.guiskin");
            
            icon_android = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_android.png");
            icon_ios = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_ios.png");
            icon_windows = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_windows.png");
            icon_mac = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_mac.png");
            
            icon_download = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_download.png");
            icon_setting = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_setting.png");
            icon_empty = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_empty.png");
            icon_check = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_check.png");
            icon_remove = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_remove.png");
            
            icon_back = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_back.png");
            icon_next = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/NhnCloud/GamebaseTools/SettingTool/Editor/UI/Skin/Textures/icon_next.png");
            
            padding_intent_12 = new GUIStyle
            {
                padding = new RectOffset(12, 0, 0, 0)
            };

            padding_intent_24 = new GUIStyle
            {
                padding = new RectOffset(24, 0, 0, 0)
            };
            
            padding_1 = new GUIStyle
            {
                padding = new RectOffset(1, 1, 1, 1)
            };
            
            padding_2 = new GUIStyle
            {
                padding = new RectOffset(2, 2, 2, 2)
            };
            
            padding_4 = new GUIStyle
            {
                padding = new RectOffset(4, 4, 4, 4)
            };

            padding_top_left_right_4 = new GUIStyle
            {
                padding = new RectOffset(4, 4, 4, 0)
            };
            
            padding_top_left_right_8 = new GUIStyle
            {
                padding = new RectOffset(8, 8, 8, 0)
            };

            padding_top_left_10 = new GUIStyle
            {
                padding = new RectOffset(10, 0, 10, 0)
            };

            padding_top_left_right_10 = new GUIStyle
            {
                padding = new RectOffset(10, 10, 10, 0)
            };

            padding_top_left_right_20 = new GUIStyle
            {
                padding = new RectOffset(20, 20, 20, 0)
            };

            padding_left_right_10 = new GUIStyle
            {
                padding = new RectOffset(10, 10, 0, 0)
            };
            padding_left_right_20 = new GUIStyle
            {
                padding = new RectOffset(20, 20, 0, 0)
            };

            SettingToolName = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                stretchWidth = false,
                fontSize = 34
            };
            
            WindowsName = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                stretchWidth = false,
                fontSize = 24
            };

            SettingToolVersion = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.LowerLeft,
                fontSize = 16
            };

            IconLabel = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(4, 4, 4, 4),
                stretchWidth = false,
                stretchHeight = false
            };

            CopyrightLabel = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 10
            };

            LanguagePopup = new GUIStyle(EditorStyles.popup)
            {
                fontSize = 11
            };

            TitleLabel = new GUIStyle(tookSkin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                //stretchWidth = false
            };

            DefaultLabel = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12
            };

            BoldLabel = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12,
                fontStyle = FontStyle.Bold
            };
            
            PageLabel = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 14,
                fontStyle = FontStyle.Bold
            };

            GUIStyleState GreenDefaultLabelStyleState = new GUIStyleState
            {
                textColor = Color.green
            };

            DefaultLabelGreen = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12,
                normal = GreenDefaultLabelStyleState,
                hover = GreenDefaultLabelStyleState,
                active = GreenDefaultLabelStyleState,
                focused = GreenDefaultLabelStyleState
            };

            GUIStyleState YellowDefaultLabelStyleState = new GUIStyleState
            {
                textColor = Color.yellow
            };
            
            DefaultLabelYellow = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12,
                normal = YellowDefaultLabelStyleState,
                hover = YellowDefaultLabelStyleState,
                active = YellowDefaultLabelStyleState,
                focused = YellowDefaultLabelStyleState
            };
            
                
            GUIStyleState RedDefaultLabelStyleState = new GUIStyleState
            {
                textColor = Color.red
            };
                
            DefaultLabelRed = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12,
                normal = RedDefaultLabelStyleState,
                hover = RedDefaultLabelStyleState,
                active = RedDefaultLabelStyleState,
                focused = RedDefaultLabelStyleState
            };

            GUIStyleState GrayDefaultLabelStyleState = new GUIStyleState
            {
                textColor = Color.gray
            };

            SmallLabel = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 11,
                normal = GrayDefaultLabelStyleState,
                hover = GrayDefaultLabelStyleState,
                active = GrayDefaultLabelStyleState,
                focused = GrayDefaultLabelStyleState
            };

            Button = new GUIStyle(tookSkin.button);

            SizeButton = new GUIStyle(tookSkin.button)
            {
                fixedWidth = 80,
                fixedHeight = 30
            };

            LinkButton = new GUIStyle(EditorStyles.linkLabel)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 11
            };

            AdapterCategory = new GUIStyle(tookSkin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fixedWidth = 229
            };

            CheckBox = new GUIStyle(tookSkin.toggle);
            CheckPartBox = new GUIStyle(tookSkin.GetStyle("check_part"));
            CheckMustBox = new GUIStyle(tookSkin.GetStyle("check_must"));
            CheckSelectedBox = new GUIStyle(tookSkin.GetStyle("check_selected"));
            CheckUpgradeBox = new GUIStyle(tookSkin.GetStyle("check_upgrade"));
            RadioBox = new GUIStyle(tookSkin.GetStyle("radio"));

            //GUI.skin.box
            Box = new GUIStyle(tookSkin.box);
            MiniBox = new GUIStyle(EditorStyles.helpBox);
            
            Popup = new GUIStyle(tookSkin.GetStyle("popup")); 
            
            TabButton = new GUIStyle(tookSkin.GetStyle("tab"));

            CheckLabel = new GUIStyle(tookSkin.GetStyle("checkLabel"));

            XLabel = new GUIStyle(tookSkin.GetStyle("xLabel"));
            
            WarningLabel = new GUIStyle(tookSkin.GetStyle("warningLabel"));
        }
    }
}

#endif