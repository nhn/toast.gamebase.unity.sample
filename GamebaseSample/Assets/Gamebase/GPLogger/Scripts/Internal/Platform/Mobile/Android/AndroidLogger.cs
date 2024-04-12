using System;
using UnityEngine;

namespace GamePlatform.Logger.Internal
{
    public class AndroidLogger : MobileLogger
    {
        private const string NHN_CLOUD_UNITY_CLASS_FULL_NAME = "com.nhncloud.android.unity.core.NhnCloudUnity";

        private AndroidJavaObject androidPlugin;

        public AndroidLogger(bool isUserAccess) : base(isUserAccess)
        {
            if (androidPlugin == null)
            {
                try
                {
                    androidPlugin = new AndroidJavaObject(NHN_CLOUD_UNITY_CLASS_FULL_NAME);
                }
                catch (Exception e)
                {
                    GpLog.Exception(e);
                }
            }
        }

        public override string SendMessage(string jsonString)
        {
            if (androidPlugin != null)
            {
                GpLog.Debug(string.Format("jsonString:{0}", jsonString), GetType(), "SendMessage");
                return androidPlugin.CallStatic<string>("unityMessage", jsonString);
            }

            return string.Empty;
        }
    }
}