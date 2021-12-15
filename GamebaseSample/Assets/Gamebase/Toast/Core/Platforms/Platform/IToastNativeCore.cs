using System.Collections.Generic;

namespace Toast.Core
{
    public interface IToastNativeCore
    {
        void Initialize();
        void SetUserId(string userId);
        string GetUserId();
        void SetDebugMode(bool debugMode);
        bool IsDebugMode();
        void SetOptionalPolicies(List<string> properties);
    }
}