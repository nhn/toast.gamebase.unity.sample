using NhnCloud.GamebaseTools.SettingTool.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public static class Multilanguage
    {
        private static int _selectedLanguageIndex;

        public static int SelectedLanguageIndex
        {
            get
            {
                return _selectedLanguageIndex;
            }
            set
            {
                _selectedLanguageIndex = value;
                EditorPrefs.SetInt(EditorPrefsKey.SETTING_TOOL_LANGUAGE, value);
            }
        }

        private static Dictionary<string, LocalizedStringVo> localizedStrings;
        private static string[] supportedLanguages;
        private static string[] supportedNativeLanguages;

        public static void Initialize()
        {
            localizedStrings = DataManager.GetData<Dictionary<string, LocalizedStringVo>>(DataKey.LOCALIZED_STRING);
            supportedLanguages = localizedStrings.Keys.ToArray();

            var languageList = new List<string>();

            for (var i = 0; i < supportedLanguages.Length; i++)
            {
                languageList.Add(CultureInfo.GetCultureInfo(supportedLanguages[i]).NativeName);
            }

            supportedNativeLanguages = languageList.ToArray();

            if (EditorPrefs.HasKey(EditorPrefsKey.SETTING_TOOL_LANGUAGE) == true)
            {
                _selectedLanguageIndex = EditorPrefs.GetInt(EditorPrefsKey.SETTING_TOOL_LANGUAGE);
            }
        }

        public static string GetString(string key)
        {
            if (localizedStrings == null)
            {
                return string.Empty;
            }

            FieldInfo fieldInfo = typeof(LocalizedStringVo).GetField(key);

            if (fieldInfo == null)
            {
                SettingToolLog.Warn(string.Format("FieldInfo not found for {0}.", key), typeof(Multilanguage), "GetString");
                return string.Empty;
            }

            return fieldInfo.GetValue(localizedStrings[supportedLanguages[SelectedLanguageIndex]]).ToString();
        }

        public static string[] GetSupportLanguages()
        {
            return supportedLanguages;
        }

        public static string[] GetSupportNativeLanguages()
        {
            return supportedNativeLanguages;
        }

        public static void Destroy()
        {
            if (localizedStrings != null)
            {
                localizedStrings.Clear();
                localizedStrings = null;
            }

            if (supportedLanguages != null)
            {
                if (supportedLanguages.Length > 0)
                {
                    Array.Clear(supportedLanguages, 0, supportedLanguages.Length);
                }

                supportedLanguages = null;
            }
        }
    }
}