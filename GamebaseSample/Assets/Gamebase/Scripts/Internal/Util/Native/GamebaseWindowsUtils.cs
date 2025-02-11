#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

using System.Runtime.InteropServices;
using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public class GamebaseWindowsUtils : IGamebaseNativeUtils
    {
        public string DeviceUniqueIdentifier
        {
            get { return SystemInfo.deviceUniqueIdentifier; }
        }

        bool IGamebaseNativeUtils.IsNetworkConnected
        {
            get
            { 
                int flags;
                return NativeBridge.InternetGetConnectedState(out flags, 0);
            }
        }

        string IGamebaseNativeUtils.TwoLetterCountryCode
        {
            get { return GamebaseCultureUtil.GetTwoLetterCountryCode((int)NativeBridge.GetSystemDefaultLCID()); }
        }

        string IGamebaseNativeUtils.TwoLetterIsoCode
        {
            get { return GamebaseCultureUtil.GetTwoLetterIsoCode((int)NativeBridge.GetSystemDefaultLCID()); }
        }
        
        public int ShowPopup(GamebasePopupInfo info)
        {
            return GamebaseWindowsMessageBox.ShowMessageBox(info);
        }
        
        private static class NativeBridge
        {
            [DllImport("kernel32.dll")]
            public static extern int GetSystemDefaultLCID();
        
            [DllImport("wininet.dll")]
            public static extern bool InternetGetConnectedState(out int flags, int reserved);
        }
    }
}

#endif