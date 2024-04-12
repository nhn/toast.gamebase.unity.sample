using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public class GamebaseSystemInfo
    {
        private const int UDID_LENGTH = 40;
        
        public static string DeviceKey { get { return GetDeviceUniqueIdentifier(); } }
        public static string UUID { get { return CreateUuid(DeviceKey); } }
        public static string CountryCode { get { return GetTwoLetterCountryCode(); } }
        public static string DeviceModel { get { return GetDeviceModel(); } }
        public static string OsVersion { get { return GetOsVersion(); } }        
        public static string DeviceLanguageCode { get { return GetTwoLetterIsoCode(); } }
        public static string Platform { get { return GetPlatform(); } }
        
        private static string deviceKey;
        private static string uuid;
        private static string countryCode;
        private static string deviceModel;
        private static string osVersion;
        private static string language;
        private static string platform;
        
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

        private static string CreateUuid(string udid)
        {
            if (string.IsNullOrEmpty(uuid) == false)
            {
                return uuid;
            }

            if (udid.Length != UDID_LENGTH)
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

        private static string GetDeviceUniqueIdentifier()
        {
            if (string.IsNullOrEmpty(deviceKey) == false && deviceKey.Equals("n/a", StringComparison.Ordinal) == false)
            {
                return deviceKey;
            }
            
            return deviceKey = GamebaseNativeUtils.Instance.DeviceUniqueIdentifier;
        }

        private static string GetTwoLetterCountryCode()
        {
            if (string.IsNullOrEmpty(countryCode) == false)
            {
                return countryCode;
            }
            
            return countryCode = GamebaseNativeUtils.Instance.TwoLetterCountryCode;
        }

        private static string GetTwoLetterIsoCode()
        {
            if (string.IsNullOrEmpty(language) == false)
            {
                return language;
            }
            
            return language = GamebaseNativeUtils.Instance.TwoLetterIsoCode;
        }
        
        private static string GetPlatform()
        {
            if (string.IsNullOrEmpty(platform) == false)
            {
                return platform;
            }
            
#if UNITY_STANDALONE_WIN
            platform = "WINDOWS";
#elif UNITY_STANDALONE_OSX
            platform = "MACOS";
#elif UNITY_WEBGL
            platform = "WEB";
#elif UNITY_IOS
            platform = "IOS";
#elif UNITY_ANDROID
            platform = "AOS";
#else
            GamebaseLog.Warn("The target platform currently set is not an officially supported platform. " +
                             "It is recognized as Windows for user convenience.", typeof(GamebaseSystemInfo));

            platform = "WINDOWS";
#endif
            return platform;
        }
    }
}