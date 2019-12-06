using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Logger
{
    public class ToastLoggerSessionFilter : IToastLoggerFilter
    {
        public bool Filter(ToastLoggerLogObject logData)
        {
            if (ToastLoggerSettings.Instance.isSession)
            {
                return true;
            }
            else
            {
                if (logData.GetLoggerType().Equals(ToastLoggerType.SESSION))
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