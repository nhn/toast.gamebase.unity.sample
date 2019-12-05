using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Logger
{
    public class ToastLoggerCrashFilter : IToastLoggerFilter
    {
        public bool Filter(ToastLoggerLogObject logData)
        {
            if (ToastLoggerSettings.Instance.isCrash)
            {
                return true;
            }
            else
            {
                if (logData.GetLoggerType().Equals(ToastLoggerType.CRASH))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
