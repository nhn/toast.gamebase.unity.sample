#if UNITY_STANDALONE || UNITY_EDITOR

using System.Collections.Generic;

namespace Toast.Core
{
    public class ToastStandaloneCore : IToastNativeCore
    {
        public void Initialize()
        {
            //ToastCoreCommonLogic.
        }

        public void SetUserId(string userId)
        {
            ToastCoreCommonLogic.UserId = userId;
        }

        public string GetUserId()
        {
            return ToastCoreCommonLogic.UserId;
        }

        public void SetDebugMode(bool debugMode)
        {
            ToastCoreCommonLogic.DebugMode = debugMode;
        }

        public bool IsDebugMode()
        {
            return ToastCoreCommonLogic.DebugMode;
        }

        public void SetOptionalPolicies(List<string> properties)
        {
            ToastCoreCommonLogic.SetOptionalPolices(properties);
        }
    }
}

#endif
