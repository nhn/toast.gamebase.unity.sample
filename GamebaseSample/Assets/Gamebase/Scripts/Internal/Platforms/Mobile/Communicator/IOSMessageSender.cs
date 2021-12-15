#if UNITY_EDITOR || UNITY_IOS
using System.Runtime.InteropServices;
using System;

namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public sealed class IOSMessageSender : INativeMessageSender
    {
		[DllImport("__Internal")]
		private static extern void initializeUnityInterface();
        [DllImport("__Internal")]
        private static extern void initialize(string className);
        [DllImport("__Internal")]
        private static extern IntPtr getSync (string jsonString);
        [DllImport("__Internal")]
        private static extern void getAsync (string jsonString);

        private static readonly IOSMessageSender instance = new IOSMessageSender();

        public static IOSMessageSender Instance
        {
            get { return instance; }
        }

        private IOSMessageSender()
        {
            GamebaseComponentManager.AddComponent<NativeMessageReceiver>(GamebaseGameObjectManager.GameObjectType.PLUGIN_TYPE);            
        }

        public string GetSync(string jsonString)
        {
            GamebaseLog.Debug(string.Format("jsonString : {0}", GamebaseJsonUtil.ToPrettyJsonString(jsonString)), this);

            string retValue = string.Empty;
            IntPtr result = getSync(jsonString);
            if (IntPtr.Zero != result)
            {
                retValue = Marshal.PtrToStringAnsi(result);
            }
            
            if (string.IsNullOrEmpty(retValue) == false)
            {
                GamebaseLog.Debug(string.Format("retValue : {0}", retValue), this);
            }

            return retValue;
        }

        public void GetAsync(string jsonString)
        {
            GamebaseLog.Debug(string.Format("jsonString : {0}", GamebaseJsonUtil.ToPrettyJsonString(jsonString)), this);

            getAsync(jsonString);
        }
        
        public void Initialize(string className)
        {
            GamebaseLog.Debug(string.Format("className : {0}", className), this);
            initialize(className);
        }

		public void InitializeUnityInterface()
		{
			initializeUnityInterface();
		}
    }
}
#endif