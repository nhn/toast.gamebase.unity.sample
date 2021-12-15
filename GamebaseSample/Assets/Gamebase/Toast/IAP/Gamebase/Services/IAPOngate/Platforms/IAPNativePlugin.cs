using UnityEngine;

namespace Toast.Iap.Ongate
{
    public abstract class IAPNativePlugin : ScriptableObject
    {
        abstract public void SetDebugMode(bool isDebuggable);
        abstract public bool RegisterUserId(string userId);
        abstract public void RequestAsyncEvent(IAPEventParam param);
    }
}