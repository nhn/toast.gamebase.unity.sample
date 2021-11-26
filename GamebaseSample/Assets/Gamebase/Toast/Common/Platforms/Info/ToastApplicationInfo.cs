using System;
using UnityEngine;

namespace Toast.Internal
{
    public static class ToastApplicationInfo
    {
        private static string _language;
        private static string _sessionId;
        private static string _launchedId; 

        public static string GetApplicationVersion()
        {
            return Application.version;
        }

        public static string GetPlatformName()
        {
            return SystemInfo.operatingSystem;
        }

        public static string GetLaunchedId()
        {
            if (string.IsNullOrEmpty(_launchedId))
            {
                _launchedId = Guid.NewGuid().ToString();
            }
            return _launchedId;
        }

        public static string GetSDKVersion()
        {
            return ToastSDKVersion.version;
        }

        public static string GetDeviceModel()
        {
            return SystemInfo.deviceModel;
        }

        public static string GetDeviceID()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        public static string GetCountryCode()
        {
            return GetCountryCodeFromSystemLanguage();
        }

        public static string GetLanguageCodeFromSystemLanguage()
        {
            if (string.IsNullOrEmpty(_language) == false)
            {
                return _language;
            }

            SystemLanguage lang = Application.systemLanguage;
            _language = "zz";
            switch (lang)
            {
                case SystemLanguage.Afrikaans: _language = "af"; break;
                case SystemLanguage.Arabic: _language = "ar"; break;
                case SystemLanguage.Basque: _language = "eu"; break;
                case SystemLanguage.Belarusian: _language = "be"; break;
                case SystemLanguage.Bulgarian: _language = "bg"; break;
                case SystemLanguage.Catalan: _language = "ca"; break;
                case SystemLanguage.Chinese: _language = "zh"; break;
                case SystemLanguage.Czech: _language = "cs"; break;
                case SystemLanguage.Danish: _language = "da"; break;
                case SystemLanguage.Dutch: _language = "nl"; break;
                case SystemLanguage.English: _language = "en"; break;
                case SystemLanguage.Estonian: _language = "et"; break;
                case SystemLanguage.Faroese: _language = "fo"; break;
                case SystemLanguage.Finnish: _language = "fi"; break;
                case SystemLanguage.French: _language = "fr"; break;
                case SystemLanguage.German: _language = "de"; break;
                case SystemLanguage.Greek: _language = "el"; break;
                case SystemLanguage.Hebrew: _language = "he"; break;
                case SystemLanguage.Icelandic: _language = "is"; break;
                case SystemLanguage.Indonesian: _language = "id"; break;
                case SystemLanguage.Italian: _language = "it"; break;
                case SystemLanguage.Japanese: _language = "jv"; break;
                case SystemLanguage.Korean: _language = "ko"; break;
                case SystemLanguage.Latvian: _language = "lv"; break;
                case SystemLanguage.Lithuanian: _language = "lt"; break;
                case SystemLanguage.Norwegian: _language = "nn"; break;
                case SystemLanguage.Polish: _language = "pl"; break;
                case SystemLanguage.Portuguese: _language = "pt"; break;
                case SystemLanguage.Romanian: _language = "ro"; break;
                case SystemLanguage.Russian: _language = "ru"; break;
                case SystemLanguage.SerboCroatian: _language = "hr"; break;
                case SystemLanguage.Slovak: _language = "sk"; break;
                case SystemLanguage.Slovenian: _language = "sl"; break;
                case SystemLanguage.Spanish: _language = "es"; break;
                case SystemLanguage.Swedish: _language = "sv"; break;
                case SystemLanguage.Thai: _language = "th"; break;
                case SystemLanguage.Turkish: _language = "tr"; break;
                case SystemLanguage.Ukrainian: _language = "uk"; break;
                case SystemLanguage.Vietnamese: _language = "vi"; break;
#if UNITY_5
                case SystemLanguage.ChineseSimplified : _language = "zh"; break;
                case SystemLanguage.ChineseTraditional : _language = "zh"; break;
#endif
                case SystemLanguage.Unknown: _language = "zz"; break;
                case SystemLanguage.Hungarian: _language = "hu"; break;

            }

            return _language;
        }

        public static string GetCountryCodeFromSystemLanguage()
        {
            if (string.IsNullOrEmpty(_language) == false)
            {
                return _language;
            }

            SystemLanguage lang = Application.systemLanguage;
            _language = "zz";
            switch (lang)
            {
                case SystemLanguage.Afrikaans: _language = "AF"; break;
                case SystemLanguage.Arabic: _language = "SA"; break;
                case SystemLanguage.Basque: _language = "ES"; break;
                case SystemLanguage.Belarusian: _language = "BY"; break;
                case SystemLanguage.Bulgarian: _language = "BG"; break;
                case SystemLanguage.Catalan: _language = "ES"; break;
                case SystemLanguage.Chinese: _language = "CN"; break;
                case SystemLanguage.Czech: _language = "CZ"; break;
                case SystemLanguage.Danish: _language = "DK"; break;
                case SystemLanguage.Dutch: _language = "DE"; break;
                case SystemLanguage.English: _language = "US"; break;
                case SystemLanguage.Estonian: _language = "EE"; break;
                case SystemLanguage.Faroese: _language = "FO"; break;
                case SystemLanguage.Finnish: _language = "FI"; break;
                case SystemLanguage.French: _language = "FR"; break;
                case SystemLanguage.German: _language = "DE"; break;
                case SystemLanguage.Greek: _language = "GR"; break;
                case SystemLanguage.Hebrew: _language = "IL"; break;
                case SystemLanguage.Icelandic: _language = "IS"; break;
                case SystemLanguage.Indonesian: _language = "ID"; break;
                case SystemLanguage.Italian: _language = "IT"; break;
                case SystemLanguage.Japanese: _language = "JP"; break;
                case SystemLanguage.Korean: _language = "KR"; break;
                case SystemLanguage.Latvian: _language = "LV"; break;
                case SystemLanguage.Lithuanian: _language = "LT"; break;
                case SystemLanguage.Norwegian: _language = "NO"; break;
                case SystemLanguage.Polish: _language = "PL"; break;
                case SystemLanguage.Portuguese: _language = "PT"; break;
                case SystemLanguage.Romanian: _language = "RO"; break;
                case SystemLanguage.Russian: _language = "RU"; break;
                case SystemLanguage.SerboCroatian: _language = "HR"; break;
                case SystemLanguage.Slovak: _language = "SK"; break;
                case SystemLanguage.Slovenian: _language = "SI"; break;
                case SystemLanguage.Spanish: _language = "ES"; break;
                case SystemLanguage.Swedish: _language = "SE"; break;
                case SystemLanguage.Thai: _language = "TH"; break;
                case SystemLanguage.Turkish: _language = "TR"; break;
                case SystemLanguage.Ukrainian: _language = "UA"; break;
                case SystemLanguage.Vietnamese: _language = "VN"; break;
#if UNITY_5
                case SystemLanguage.ChineseSimplified : _language = "zh"; break;
                case SystemLanguage.ChineseTraditional : _language = "zh"; break;
#endif
                case SystemLanguage.Unknown: _language = "ZZ"; break;
                case SystemLanguage.Hungarian: _language = "HU"; break;

            }

            return _language;
        }

        public static string GetSessionId()
        {
            if (string.IsNullOrEmpty(_sessionId))
            {
                _sessionId = Guid.NewGuid().ToString();
            }
            return _sessionId;
        }

        public static string GetAuditKey()
        {
            return "6WYMWnQLwJwGjpot";
        }
    }
}