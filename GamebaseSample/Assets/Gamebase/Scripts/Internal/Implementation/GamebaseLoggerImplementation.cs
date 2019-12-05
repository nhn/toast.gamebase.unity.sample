#if !UNITY_EDITOR && UNITY_ANDROID
using Toast.Gamebase.Mobile.Android;
#elif !UNITY_EDITOR && UNITY_IOS
using Toast.Gamebase.Mobile.IOS;
#elif !UNITY_EDITOR && UNITY_WEBGL
using Toast.Gamebase.Single.WebGL;
#else
using Toast.Gamebase.Single.Standalone;
#endif

using System.Collections.Generic;

namespace Toast.Gamebase
{
    public sealed class GamebaseLoggerImplementation
    {        
        private const string UNITY_EDITOR_VERSION = "UNITY_EDITOR_VERSION";
        public const string SEND_DATA_KEY_UNITY_EDITOR_VERSION = "UnityEditorVersion";
        public const string SEND_DATA_KEY_UNITY_SDK_VERSION = "UnitySDKVersion";
        public const string SEND_DATA_KEY_PLATFORM_SDK_VERSION = "PlatformSDKVersion";

        private static readonly GamebaseLoggerImplementation instance = new GamebaseLoggerImplementation();

        public static GamebaseLoggerImplementation Instance
        {
            get { return instance; }
        }

        private IGamebaseLogger sdk;

        private GamebaseLoggerImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            sdk = new AndroidGamebaseLogger();
#elif !UNITY_EDITOR && UNITY_IOS
            sdk = new IOSGamebaseLogger();
#elif !UNITY_EDITOR && UNITY_WEBGL
            sdk = new WebGLGamebaseLogger();
#else
            sdk = new StandaloneGamebaseLogger();
#endif
        }

        public void Initialize(GamebaseRequest.Logger.Configuration loggerConfiguration)
        {
            sdk.Initialize(loggerConfiguration);
        }

        public void Debug(string message, Dictionary<string, string> userFields = null)
        {
            sdk.Debug(message, userFields);
        }

        public void Info(string message, Dictionary<string, string> userFields = null)
        {
            sdk.Info(message, userFields);
        }

        public void Warn(string message, Dictionary<string, string> userFields = null)
        {
            sdk.Warn(message, userFields);
        }

        public void Error(string message, Dictionary<string, string> userFields = null)
        {
            sdk.Error(message, userFields);
        }

        public void Fatal(string message, Dictionary<string, string> userFields = null)
        {
            sdk.Fatal(message, userFields);
        }

        public void SetUserField(string key, string value)
        {
            sdk.SetUserField(key, value);
        }

        public void SetLoggerListener(GamebaseCallback.Logger.ILoggerListener listener)
        {
            sdk.SetLoggerListener(listener);
        }

        public void SetCrashListener(GamebaseCallback.Logger.CrashListener listener)
        {
            sdk.SetCrashListener(listener);
        }

        public void AddCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
        {
            sdk.AddCrashFilter(filter);
        }

        public void RemoveCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
        {
            sdk.RemoveCrashFilter(filter);
        }
    }
}