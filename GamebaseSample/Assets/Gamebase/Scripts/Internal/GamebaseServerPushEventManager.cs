using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal
{
    public class GamebaseServerPushEventManager
    {
        private static readonly GamebaseServerPushEventManager instance = new GamebaseServerPushEventManager();

        public static GamebaseServerPushEventManager Instance
        {
            get { return instance; }
        }

        private HashSet<GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage>> serverPushEventSet = new HashSet<GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage>>();

        public int Handle
        {
            get;
            internal set;
        }

        private GamebaseServerPushEventManager()
        {
            Handle = GamebaseCallbackHandler.RegisterCallback(new GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage>(OnServerPushEvent));
        }

        public void AddServerPushEvent(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage> serverPushEvent)
        {
            if (false == serverPushEventSet.Add(serverPushEvent))
            {
                GamebaseLog.Warn(GamebaseStrings.ADD_SERVER_PUSH_FAILED, this);
                return;
            }
        }

        public void RemoveServerPushEvent(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage> serverPushEvent)
        {
            if (false == serverPushEventSet.Remove(serverPushEvent))
            {
                GamebaseLog.Warn(GamebaseStrings.REMOVE_SERVER_PUSH_FAILED, this);
                return;
            }
        }

        public void RemoveAllServerPushEvent()
        {
            if (null == serverPushEventSet)
            {
                return;
            }

            if (0 == serverPushEventSet.Count)
            {
                return;
            }

            serverPushEventSet.Clear();
        }

        public int GetCount()
        {
            if (null == serverPushEventSet)
            {
                return 0;
            }

            return serverPushEventSet.Count;
        }

        public void OnServerPushEvent(GamebaseResponse.SDK.ServerPushMessage message)
        {
            if (null != serverPushEventSet && 0 < serverPushEventSet.Count)
            {
                foreach (var serverPushEvent in serverPushEventSet)
                {
                    if (null != serverPushEvent)
                    {
                        serverPushEvent(message);
                    }
                }
            }
        }
    }
}