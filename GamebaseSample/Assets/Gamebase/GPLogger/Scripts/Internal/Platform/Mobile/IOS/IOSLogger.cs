#if UNITY_IOS || UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace GamePlatform.Logger.Internal
{
    public class IOSLogger : MobileLogger
    {
        public IOSLogger(bool isUserAccess) : base(isUserAccess)
        {
        }

        [DllImport("__Internal")]
        private static extern IntPtr GPLoggerPlugin_receiveMessageFromEngine(string data);

        public override string SendMessage(string jsonString)
        {
            GpLog.Debug(string.Format("jsonString:{0}", jsonString), GetType(), "SendMessage");

            var ret = GPLoggerPlugin_receiveMessageFromEngine(jsonString);
            return Marshal.PtrToStringAnsi(ret);
        }
    }
}
#endif