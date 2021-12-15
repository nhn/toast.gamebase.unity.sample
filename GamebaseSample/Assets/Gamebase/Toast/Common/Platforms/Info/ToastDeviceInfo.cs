using System;
using UnityEngine;

namespace Toast.Internal
{
    public static class ToastDeviceInfo
    {
        private const string TOASTSDK_UDID = "ToastSDK.udid";

        private static string _deviceUDID;

        public static string GetDeviceUniqueIdentifier()
        {
            if (string.IsNullOrEmpty(_deviceUDID) == false && _deviceUDID.Equals("n/a", StringComparison.Ordinal))
            {
                return _deviceUDID;
            }

            string guid = PlayerPrefs.GetString(TOASTSDK_UDID);
            if (true == string.IsNullOrEmpty(guid))
            {
                guid = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(TOASTSDK_UDID, guid);
                PlayerPrefs.Save();
            }

            _deviceUDID = guid;

            return _deviceUDID;
        }
    }
}