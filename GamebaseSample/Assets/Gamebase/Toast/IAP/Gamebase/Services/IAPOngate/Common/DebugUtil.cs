using System.Collections.Generic;
using UnityEngine;

namespace Toast.Iap.Ongate
{
    public sealed class DebugUtil : ScriptableObject
    {
        public delegate void LogListener(string msg);
        private static List<LogListener> afterLogLinsteners;
        private static bool enableDebug;
        public static bool EnableDebug
        {
            get { return enableDebug; }
            set { enableDebug = value; }
        }

        static DebugUtil()
        {
            afterLogLinsteners = new List<LogListener>();
            enableDebug = (Debug.isDebugBuild) ? true : false;
        }

        public static void AddAfterLogListener(LogListener listener)
        {
            afterLogLinsteners.Add(listener);
        }

        public static void RemoveAfterLogListener(LogListener listener)
        {
            afterLogLinsteners.Remove(listener);
        }

        public static void Log(string msg)
        {
            if (enableDebug)
            {
                Debug.Log(msg);
                afterLogLinsteners.ForEach(listener => listener(msg));
            }
        }
    }
}