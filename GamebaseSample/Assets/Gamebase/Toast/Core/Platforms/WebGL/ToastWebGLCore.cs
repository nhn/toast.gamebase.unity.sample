#if UNITY_WEBGL

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Core
{
    public class ToastWebGLCore : IToastNativeCore
    {
        public void Initialize()
        {

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
