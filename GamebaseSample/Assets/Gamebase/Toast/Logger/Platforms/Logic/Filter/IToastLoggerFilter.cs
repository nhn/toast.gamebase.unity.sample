using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Logger
{
    public interface IToastLoggerFilter
    { 
        bool Filter(ToastLoggerLogObject logData);
    }
}