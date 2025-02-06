using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class LocalizedString
    {
        public Dictionary<string, string> localize = new Dictionary<string, string>();

        public void Add(LocalizedString addData)
        {
            foreach(var pair in addData.localize)
            {
                if(localize.ContainsKey(pair.Key))
                {
                    localize[pair.Key] = pair.Value;
                }
                else
                {
                    localize.Add(pair.Key, pair.Value);
                }
            }
        }
    }

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

        private static Dictionary<string, LocalizedString> localizedStrings;
        private static string[] supportedLanguages;
        private static string[] supportedNativeLanguages;

        public static void Initialize()
        {
            if (EditorPrefs.HasKey(EditorPrefsKey.SETTING_TOOL_LANGUAGE))
            {
                _selectedLanguageIndex = EditorPrefs.GetInt(EditorPrefsKey.SETTING_TOOL_LANGUAGE);
            }
        }

        public static void SetLocalizedStrings(Dictionary<string, LocalizedString> value)
        {
            localizedStrings = value;
            supportedLanguages = localizedStrings.Keys.ToArray();

            var languageList = new List<string>();

            for (var i = 0; i < supportedLanguages.Length; i++)
            {
                languageList.Add(CultureInfo.GetCultureInfo(supportedLanguages[i]).NativeName);
            }

            supportedNativeLanguages = languageList.ToArray();
        }

        public static void AddLocalizedStrings(Dictionary<string, LocalizedString> value)
        {
            if (localizedStrings == null)
            {
                SetLocalizedStrings(value);
            }
            else
            {
                var supportedLanguagesList = new List<string>(supportedLanguages);
                foreach (var pair in value)
                {
                    if (localizedStrings.ContainsKey(pair.Key))
                    {
                        localizedStrings[pair.Key].Add(pair.Value);
                    }
                    else
                    {
                        localizedStrings.Add(pair.Key, pair.Value);

                        supportedLanguagesList.Add(pair.Key);
                    }
                }

                supportedLanguages = supportedLanguagesList.ToArray();

                var languageList = new List<string>();
                for (var i = 0; i < supportedLanguages.Length; i++)
                {
                    languageList.Add(CultureInfo.GetCultureInfo(supportedLanguages[i]).NativeName);
                }
                supportedNativeLanguages = languageList.ToArray();
            }
        }

        public static string GetString(string key)
        {
            if (localizedStrings == null ||
                string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            LocalizedString selectedLocalized = localizedStrings[supportedLanguages[SelectedLanguageIndex]];

            string value = null;
            if(selectedLocalized.localize.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return key;
            }
        }

        public static string GetString(string key, params object[] args)
        {
            return string.Format(GetString(key), args);
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