#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
using UnityEngine;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeMessageReceiver : MonoBehaviour
    {
        public void OnAsyncEvent(string jsonString)
        {
            GamebaseLog.Debug(string.Format("jsonString : {0}", jsonString), this);

            NativeMessage message = JsonMapper.ToObject<NativeMessage>(jsonString);

            if(message == null)
            {
                GamebaseLog.Debug("JSON parsing error. message object is null.", this);
                return;
            }

            DelegateManager.DelegateData delegateData = DelegateManager.GetDelegate(message.scheme);

            if (delegateData != null)
            {
                if (delegateData.pluginEventDelegate != null)
                {
                    delegateData.pluginEventDelegate(message);
                }
                if (delegateData.eventDelegate != null)
                {
                    delegateData.eventDelegate(message);
                }
            }
        }

        public void OnLogReport(string message)
        {
            GamebaseLog.Debug(string.Format("message : {0}", message), this);
            
            GamebaseInternalReport.Instance.SendPluginLog(
                new System.Collections.Generic.Dictionary<string, string>
                {
                    {"GAMEBASE_LOG", message},
                });
        }
    }
}
#endif