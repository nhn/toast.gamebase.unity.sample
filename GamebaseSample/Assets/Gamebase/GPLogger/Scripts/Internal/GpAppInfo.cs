using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GamePlatform.Logger.Internal
{
    public static class GpAppInfo
    {
        public const string DEFAULT_LOG_SOURCE = "gp-sdk";

        public static readonly string platform = SystemInfo.operatingSystem;
        public static readonly string sessionId = Guid.NewGuid().ToString();
        public static readonly string launchedId = Guid.NewGuid().ToString();
        public static readonly string deviceModel = SystemInfo.deviceModel;

        public static readonly string deviceId;
        public static readonly string languageCode;
        public static readonly string countryCode;

        static GpAppInfo()
        {
            deviceId = GetDeviceId();
            languageCode = Get2LetterISOCodeFromSystemLanguage();
            countryCode = Get2LetterCountryCodeFromSystemLanguage();
        }

        private static string GetDeviceId()
        {
            const string GP_UDID = "GamePlatform.udid";

            var guid = PlayerPrefs.GetString(GP_UDID);
            if (string.IsNullOrEmpty(guid) == true)
            {
                guid = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(GP_UDID, guid);
                PlayerPrefs.Save();
            }

            return guid;
        }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        [DllImport("KERNEL32.DLL")]
        private static extern int GetSystemDefaultLCID();
#endif
        private static string Get2LetterISOCodeFromSystemLanguage()
        {
            var code = string.Empty;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            CultureInfo currentCulture = new CultureInfo(GetSystemDefaultLCID());
            code = currentCulture.TwoLetterISOLanguageName;

            if (string.IsNullOrEmpty(code) == true)
            {
                code = "zz";
            }

            return code;
#else
            SystemLanguage lang = Application.systemLanguage;
            code = "zz";
            switch (lang)
            {
                case SystemLanguage.Afrikaans: code = "af"; break;
                case SystemLanguage.Arabic: code = "ar"; break;
                case SystemLanguage.Basque: code = "eu"; break;
                case SystemLanguage.Belarusian: code = "be"; break;
                case SystemLanguage.Bulgarian: code = "bg"; break;
                case SystemLanguage.Catalan: code = "ca"; break;
                case SystemLanguage.Chinese: code = "zh"; break;
                case SystemLanguage.Czech: code = "cs"; break;
                case SystemLanguage.Danish: code = "da"; break;
                case SystemLanguage.Dutch: code = "nl"; break;
                case SystemLanguage.English: code = "en"; break;
                case SystemLanguage.Estonian: code = "et"; break;
                case SystemLanguage.Faroese: code = "fo"; break;
                case SystemLanguage.Finnish: code = "fi"; break;
                case SystemLanguage.French: code = "fr"; break;
                case SystemLanguage.German: code = "de"; break;
                case SystemLanguage.Greek: code = "el"; break;
                case SystemLanguage.Hebrew: code = "he"; break;
                case SystemLanguage.Hungarian: code = "hu"; break;
                case SystemLanguage.Icelandic: code = "is"; break;
                case SystemLanguage.Indonesian: code = "id"; break;
                case SystemLanguage.Italian: code = "it"; break;
                case SystemLanguage.Japanese: code = "ja"; break;
                case SystemLanguage.Korean: code = "ko"; break;
                case SystemLanguage.Latvian: code = "lv"; break;
                case SystemLanguage.Lithuanian: code = "lt"; break;
                case SystemLanguage.Norwegian: code = "no"; break;
                case SystemLanguage.Polish: code = "pl"; break;
                case SystemLanguage.Portuguese: code = "pt"; break;
                case SystemLanguage.Romanian: code = "ro"; break;
                case SystemLanguage.Russian: code = "ru"; break;
                case SystemLanguage.SerboCroatian: code = "sh"; break;
                case SystemLanguage.Slovak: code = "sk"; break;
                case SystemLanguage.Slovenian: code = "sl"; break;
                case SystemLanguage.Spanish: code = "es"; break;
                case SystemLanguage.Swedish: code = "sv"; break;
                case SystemLanguage.Thai: code = "th"; break;
                case SystemLanguage.Turkish: code = "tr"; break;
                case SystemLanguage.Ukrainian: code = "uk"; break;
                case SystemLanguage.Unknown: code = "en"; break;
                case SystemLanguage.Vietnamese: code = "vi"; break;
                case SystemLanguage.ChineseSimplified: code = "zh"; break;
                case SystemLanguage.ChineseTraditional: code = "zh"; break;
            }

            return code;
#endif
        }

        private static string Get2LetterCountryCodeFromSystemLanguage()
        {
            var code = string.Empty;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            CultureInfo currentCulture = new CultureInfo(GetSystemDefaultLCID());
            string currentCultureName = currentCulture.Name;
            int index = currentCultureName.IndexOf('-');

            if (0 < index && index + 1 < currentCultureName.Length)
            {
                code = currentCultureName.Substring(index + 1);
            }

            if (string.IsNullOrEmpty(code) == true)
            {
                code = "ZZ";
            }

            return code;
#else
            SystemLanguage lang = Application.systemLanguage;
            code = "ZZ";
            switch (lang)
            {
                case SystemLanguage.Afrikaans: code = "ZA"; break;
                case SystemLanguage.Arabic: code = "SA"; break;
                case SystemLanguage.Basque: code = "ES"; break;
                case SystemLanguage.Belarusian: code = "BY"; break;
                case SystemLanguage.Bulgarian: code = "BG"; break;
                case SystemLanguage.Catalan: code = "ES"; break;
                case SystemLanguage.Chinese: code = "CN"; break;
                case SystemLanguage.Czech: code = "CZ"; break;
                case SystemLanguage.Danish: code = "DK"; break;
                case SystemLanguage.Dutch: code = "NL"; break;
                case SystemLanguage.English: code = "US"; break;
                case SystemLanguage.Estonian: code = "EE"; break;
                case SystemLanguage.Faroese: code = "FO"; break;
                case SystemLanguage.Finnish: code = "FI"; break;
                case SystemLanguage.French: code = "FR"; break;
                case SystemLanguage.German: code = "DE"; break;
                case SystemLanguage.Greek: code = "GR"; break;
                case SystemLanguage.Hebrew: code = "IL"; break;
                case SystemLanguage.Hungarian: code = "HU"; break;
                case SystemLanguage.Icelandic: code = "IS"; break;
                case SystemLanguage.Indonesian: code = "ID"; break;
                case SystemLanguage.Italian: code = "IT"; break;
                case SystemLanguage.Japanese: code = "JP"; break;
                case SystemLanguage.Korean: code = "KR"; break;
                case SystemLanguage.Latvian: code = "LV"; break;
                case SystemLanguage.Lithuanian: code = "LT"; break;
                case SystemLanguage.Norwegian: code = "NO"; break;
                case SystemLanguage.Polish: code = "PL"; break;
                case SystemLanguage.Portuguese: code = "PT"; break;
                case SystemLanguage.Romanian: code = "RO"; break;
                case SystemLanguage.Russian: code = "RU"; break;
                case SystemLanguage.SerboCroatian: code = "RS"; break;
                case SystemLanguage.Slovak: code = "SK"; break;
                case SystemLanguage.Slovenian: code = "SI"; break;
                case SystemLanguage.Spanish: code = "ES"; break;
                case SystemLanguage.Swedish: code = "SE"; break;
                case SystemLanguage.Thai: code = "TH"; break;
                case SystemLanguage.Turkish: code = "TR"; break;
                case SystemLanguage.Ukrainian: code = "UA"; break;
                case SystemLanguage.Unknown: code = "US"; break;
                case SystemLanguage.Vietnamese: code = "VN"; break;
                case SystemLanguage.ChineseSimplified: code = "CN"; break;
                case SystemLanguage.ChineseTraditional: code = "CN"; break;
            }

            return code;
#endif
        }
    }
}