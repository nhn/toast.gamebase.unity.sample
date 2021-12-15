using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public static class GamebaseUnitySDK
    {
        public const string SDK_VERSION = "2.30.0";

        public static bool IsInitialized { get; set; }
        public static string SDKVersion { get { return SDK_VERSION; } }
        public static string AppID { get; set; }
        public static string AppVersion { get; set; }
        public static string DisplayLanguageCode { get; set; }
        public static string ZoneType { get; set; }                
        public static bool EnablePopup { get; set; }
        public static bool EnableLaunchingStatusPopup { get; set; }
        public static bool EnableBanPopup { get; set; }
        public static bool EnableKickoutPopup { get; set; }
        public static string StoreCode { get; set; }
        public static string FcmSenderId { get; set; }
        public static bool UseWebViewLogin { get; set; }

        #region system api
        public static string DeviceKey { get { return GetDeviceUniqueIdentifier(); } }
        public static string UUID { get { return CreateUUID(DeviceKey); } }
        public static string CountryCode { get { return Get2LetterCountryCodeFromSystemLanguage(); } }
        public static string DeviceModel { get { return GetDeviceModel(); } }
        public static string OsVersion { get { return GetOsVersion(); } }        
        public static string DeviceLanguageCode { get { return Get2LetterISOCodeFromSystemLanguage(); } }
        public static string Platform { get { return GetPlatform(); } }
        #endregion

        #region Internel API
        private static string deviceKey;
        private static string uuid;
        private static string countryCode;
        private static string deviceModel;
        private static string osVersion;
        private static string language;
        private static string platform;

        private static string GetDeviceUniqueIdentifier()
        {
            if (string.IsNullOrEmpty(deviceKey) == false &&deviceKey.Equals("n/a", StringComparison.Ordinal) == false)
            {
                return deviceKey;
            }
            
#if UNITY_EDITOR || UNITY_STANDALONE
            deviceKey = SystemInfo.deviceUniqueIdentifier;
#elif UNITY_WEBGL
            string guid = PlayerPrefs.GetString("Gamebase.WebGL.udid");

            if (true == string.IsNullOrEmpty(guid))
            {
                guid = Guid.NewGuid().ToString();
                PlayerPrefs.SetString("Gamebase.WebGL.udid", guid);
                PlayerPrefs.Save();
            }
            
            deviceKey = guid;
#else
            deviceKey = string.Empty;
#endif

            return deviceKey;
        }

        private static string CreateUUID(string udid)
        {
            if (string.IsNullOrEmpty(uuid) == false)
            {
                return uuid;
            }

            if (udid.Length != 40)
            {
                SHA1 sha = new SHA1CryptoServiceProvider();
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(udid));

                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                udid = sb.ToString();
            }

            //Change size 20 bytes to 10 bytes using XOR(string length 40 -> 20)
            byte[] buffer = new byte[10];

            for (int i = 0; i < buffer.Length; i++)
            {
                byte preByte = Convert.ToByte(udid.Substring(i * 2, 2), 16);
                byte postByte = Convert.ToByte(udid.Substring(i * 2 + 20, 2), 16);

                buffer[i] = (byte)(preByte ^ postByte);
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }

            return uuid = builder.ToString();
        }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        [DllImport("KERNEL32.DLL")]
        private static extern int GetSystemDefaultLCID();
#endif

        private static string Get2LetterCountryCodeFromSystemLanguage()
        {
            if (string.IsNullOrEmpty(countryCode) == false)
            {
                return countryCode;
            }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            CultureInfo currentCulture = new CultureInfo(GetSystemDefaultLCID());
            string currentCultureName = currentCulture.Name;
            int index = currentCultureName.IndexOf('-');

            if (0 < index && index + 1 < currentCultureName.Length)
            {
                countryCode = currentCultureName.Substring(index + 1);
            }

            if (string.IsNullOrEmpty(countryCode) == true)
            {
                countryCode = "ZZ";
            }

            return countryCode;
#else
            SystemLanguage lang = Application.systemLanguage;
            countryCode = "ZZ";
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
                case SystemLanguage.ChineseSimplified: countryCode = "CN"; break;
                case SystemLanguage.ChineseTraditional: countryCode = "CN"; break;
            }

            return countryCode;
#endif
        }

        private static string GetDeviceModel()
        {
            if (string.IsNullOrEmpty(deviceModel) == false)
            {
                return deviceModel;
            }

            return deviceModel = SystemInfo.deviceModel;
        }

        private static string GetOsVersion()
        {
            if (string.IsNullOrEmpty(osVersion) == false)
            {
                return osVersion;
            }

            return osVersion = SystemInfo.operatingSystem;
        }

        private static string Get2LetterISOCodeFromSystemLanguage()
        {
            if (string.IsNullOrEmpty(language) == false)
            {
                return language;
            }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            CultureInfo currentCulture = new CultureInfo(GetSystemDefaultLCID());
            language = currentCulture.TwoLetterISOLanguageName;

            if (string.IsNullOrEmpty(language) == true)
            {
                language = "zz";
            }

            return language;
#else
            SystemLanguage lang = Application.systemLanguage;
            language = "zz";
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
                case SystemLanguage.ChineseSimplified: language = "zh"; break;
                case SystemLanguage.ChineseTraditional: language = "zh"; break;
            }

            return language;
#endif
        }

        private static string GetPlatform()
        {
            if (string.IsNullOrEmpty(platform) == false)
            {
                return platform;
            }

#if UNITY_STANDALONE
            platform = "WINDOWS";
#elif UNITY_WEBGL
            platform = "WEB";
#elif UNITY_IOS
            platform = "IOS";
#elif UNITY_ANDROID
            platform = "AOS";
#else
            platform = "WINDOWS";
#endif
            return platform;
        }
#endregion
    }
}