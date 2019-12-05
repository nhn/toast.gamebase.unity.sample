using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Internal
{
    public interface IToastNativeInstanceLogger
    {
        void Initialize(ServiceZone serviceZone);
        void Log(string type, string logLevel, string message, Dictionary<string, string> userFields);
        void SetUserField(string key, string value);
        void SetAppKey(string appKey);        
    }
}
