using System.Collections.Generic;

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
            if (serverPushEventSet.Add(serverPushEvent) == false)
            {
                GamebaseLog.Warn(GamebaseStrings.ADD_SERVER_PUSH_FAILED, this);
                return;
            }
        }

        public void RemoveServerPushEvent(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage> serverPushEvent)
        {
            if (serverPushEventSet.Remove(serverPushEvent) == false)
            {
                GamebaseLog.Warn(GamebaseStrings.REMOVE_SERVER_PUSH_FAILED, this);
                return;
            }
        }

        public void RemoveAllServerPushEvent()
        {
            if (serverPushEventSet == null)
            {
                return;
            }

            if (serverPushEventSet.Count == 0)
            {
                return;
            }

            serverPushEventSet.Clear();
        }

        public int GetCount()
        {
            if (serverPushEventSet == null)
            {
                return 0;
            }

            return serverPushEventSet.Count;
        }

        public void OnServerPushEvent(GamebaseResponse.SDK.ServerPushMessage message)
        {
            if (serverPushEventSet != null && serverPushEventSet.Count > 0)
            {
                foreach (var serverPushEvent in serverPushEventSet)
                {
                    if (serverPushEvent != null)
                    {
                        serverPushEvent(message);
                    }
                }
            }
        }
    }
}