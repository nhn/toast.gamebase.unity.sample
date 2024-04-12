﻿using UnityEngine;

namespace GamePlatform.Logger
{
    public class CrashLogData
    {
        public CrashLogData(LogType logType, string condition, string stackTrace)
        {
            LogType = logType;
            Condition = condition;
            StackTrace = stackTrace;
        }

        public LogType LogType { get; private set; }

        public string Condition { get; private set; }

        public string StackTrace { get; private set; }
    }
}