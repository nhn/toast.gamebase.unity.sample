using UnityEngine;

namespace Toast.Iap.Ongate
{
    public class LocationUtil
    {
        private static string language;
        private static string countryCode;

        public static string Location
        {
            get
            {
                if (!string.IsNullOrEmpty(countryCode))
                {
                    return countryCode;
                }
                else
                {
                    return null;
                }
            }
        }

        static LocationUtil()
        {
            language = Get2LetterISOCodeFromSystemLanguage();
            countryCode = Get2LetterCountryCodeFromSystemLanguage();
        }
        private static string Get2LetterISOCodeFromSystemLanguage()
        {
            if (string.IsNullOrEmpty(language) == false)
                return language;

            SystemLanguage lang = Application.systemLanguage;
            language = "ko";
            switch (lang)
            {
                case SystemLanguage.Afrikaans: language = "af"; break;
                case SystemLanguage.Arabic: language = "ar"; break;
                case SystemLanguage.Basque: language = "eu"; break;
                case SystemLanguage.Belarusian: language = "be"; break;
                case SystemLanguage.Bulgarian: language = "bg"; break;
                case SystemLanguage.Catalan: language = "ca"; break;
                case SystemLanguage.Chinese: language = "zh"; break;
                case SystemLanguage.Czech: language = "cs"; break;
                case SystemLanguage.Danish: language = "da"; break;
                case SystemLanguage.Dutch: language = "nl"; break;
                case SystemLanguage.English: language = "en"; break;
                case SystemLanguage.Estonian: language = "et"; break;
                case SystemLanguage.Faroese: language = "fo"; break;
                case SystemLanguage.Finnish: language = "fi"; break;
                case SystemLanguage.French: language = "fr"; break;
                case SystemLanguage.German: language = "de"; break;
                case SystemLanguage.Greek: language = "el"; break;
                case SystemLanguage.Hebrew: language = "he"; break;
                case SystemLanguage.Hungarian: language = "hu"; break;
                case SystemLanguage.Icelandic: language = "is"; break;
                case SystemLanguage.Indonesian: language = "id"; break;
                case SystemLanguage.Italian: language = "it"; break;
                case SystemLanguage.Japanese: language = "ja"; break;
                case SystemLanguage.Korean: language = "ko"; break;
                case SystemLanguage.Latvian: language = "lv"; break;
                case SystemLanguage.Lithuanian: language = "lt"; break;
                case SystemLanguage.Norwegian: language = "no"; break;
                case SystemLanguage.Polish: language = "pl"; break;
                case SystemLanguage.Portuguese: language = "pt"; break;
                case SystemLanguage.Romanian: language = "ro"; break;
                case SystemLanguage.Russian: language = "ru"; break;
                case SystemLanguage.SerboCroatian: language = "sh"; break;
                case SystemLanguage.Slovak: language = "sk"; break;
                case SystemLanguage.Slovenian: language = "sl"; break;
                case SystemLanguage.Spanish: language = "es"; break;
                case SystemLanguage.Swedish: language = "sv"; break;
                case SystemLanguage.Thai: language = "th"; break;
                case SystemLanguage.Turkish: language = "tr"; break;
                case SystemLanguage.Ukrainian: language = "uk"; break;
                case SystemLanguage.Unknown: language = "en"; break;
                case SystemLanguage.Vietnamese: language = "vi"; break;
#if UNITY_5
                case SystemLanguage.ChineseSimplified: language = "zh"; break;
                case SystemLanguage.ChineseTraditional: language = "zh"; break;
#endif
            }

            return language;
        }

        private static string Get2LetterCountryCodeFromSystemLanguage()
        {
            if (string.IsNullOrEmpty(countryCode) == false)
                return countryCode;

            SystemLanguage lang = Application.systemLanguage;
            countryCode = "KR";
            switch (lang)
            {
                case SystemLanguage.Afrikaans: countryCode = "ZA"; break;
                case SystemLanguage.Arabic: countryCode = "SA"; break;
                case SystemLanguage.Basque: countryCode = "ES"; break;
                case SystemLanguage.Belarusian: countryCode = "BY"; break;
                case SystemLanguage.Bulgarian: countryCode = "BG"; break;
                case SystemLanguage.Catalan: countryCode = "ES"; break;
                case SystemLanguage.Chinese: countryCode = "CN"; break;
                case SystemLanguage.Czech: countryCode = "CZ"; break;
                case SystemLanguage.Danish: countryCode = "DK"; break;
                case SystemLanguage.Dutch: countryCode = "NL"; break;
                case SystemLanguage.English: countryCode = "US"; break;
                case SystemLanguage.Estonian: countryCode = "EE"; break;
                case SystemLanguage.Faroese: countryCode = "FO"; break;
                case SystemLanguage.Finnish: countryCode = "FI"; break;
                case SystemLanguage.French: countryCode = "FR"; break;
                case SystemLanguage.German: countryCode = "DE"; break;
                case SystemLanguage.Greek: countryCode = "GR"; break;
                case SystemLanguage.Hebrew: countryCode = "IL"; break;
                case SystemLanguage.Hungarian: countryCode = "HU"; break;
                case SystemLanguage.Icelandic: countryCode = "IS"; break;
                case SystemLanguage.Indonesian: countryCode = "ID"; break;
                case SystemLanguage.Italian: countryCode = "IT"; break;
                case SystemLanguage.Japanese: countryCode = "JP"; break;
                case SystemLanguage.Korean: countryCode = "KR"; break;
                case SystemLanguage.Latvian: countryCode = "LV"; break;
                case SystemLanguage.Lithuanian: countryCode = "LT"; break;
                case SystemLanguage.Norwegian: countryCode = "NO"; break;
                case SystemLanguage.Polish: countryCode = "PL"; break;
                case SystemLanguage.Portuguese: countryCode = "PT"; break;
                case SystemLanguage.Romanian: countryCode = "RO"; break;
                case SystemLanguage.Russian: countryCode = "RU"; break;
                case SystemLanguage.SerboCroatian: countryCode = "RS"; break;
                case SystemLanguage.Slovak: countryCode = "SK"; break;
                case SystemLanguage.Slovenian: countryCode = "SI"; break;
                case SystemLanguage.Spanish: countryCode = "ES"; break;
                case SystemLanguage.Swedish: countryCode = "SE"; break;
                case SystemLanguage.Thai: countryCode = "TH"; break;
                case SystemLanguage.Turkish: countryCode = "TR"; break;
                case SystemLanguage.Ukrainian: countryCode = "UA"; break;
                case SystemLanguage.Unknown: countryCode = "US"; break;
                case SystemLanguage.Vietnamese: countryCode = "VN"; break;
#if UNITY_5
                case SystemLanguage.ChineseSimplified: countryCode = "CN"; break;
                case SystemLanguage.ChineseTraditional: countryCode = "CN"; break;
#endif
            }

            return countryCode;
        }
    }
}
