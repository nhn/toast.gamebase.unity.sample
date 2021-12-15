#if UNITY_EDITOR || UNITY_ANDROID
using UnityEngine;

namespace Toast.Gamebase.Internal.Mobile.Android
{
    public sealed class AndroidMessageSender : INativeMessageSender
    {
        private static readonly AndroidMessageSender instance = new AndroidMessageSender();
        private const string GAMEBASE_ANDROID_PLUGIN_CLASS = "com.toast.android.gamebase.unity.communicator.UnityMessageReceiver";
        private AndroidJavaClass jc = null;

        public static AndroidMessageSender Instance
        {
            get { return instance; }
        }

        private AndroidMessageSender()
        {
            if (jc == null)
            {
                jc = new AndroidJavaClass(GAMEBASE_ANDROID_PLUGIN_CLASS);
            }

            GamebaseComponentManager.AddComponent<NativeMessageReceiver>(GamebaseGameObjectManager.GameObjectType.PLUGIN_TYPE);
        }

        public string GetSync(string jsonString)
        {
            GamebaseLog.Debug(string.Format("jsonString : {0}", GamebaseJsonUtil.ToPrettyJsonString(jsonString)), this);
            
            string retValue = jc.CallStatic<string>("getSync", jsonString);

            if (string.IsNullOrEmpty(retValue) == false)
            {
                GamebaseLog.Debug(string.Format("retValue : {0}", retValue), this);                
            }

            return retValue;
        }

        public void GetAsync(string jsonString)
        {
            GamebaseLog.Debug(string.Format("jsonString : {0}", GamebaseJsonUtil.ToPrettyJsonString(jsonString)), this);

            jc.CallStatic("getAsync", jsonString);
        }

        public void Initialize(string className)
        {
            GamebaseLog.Debug(string.Format("className : {0}", className), this);
            jc.CallStatic("initialize", className);
        }

		public void InitializeUnityInterface()
		{
		}
    }
}
#endif