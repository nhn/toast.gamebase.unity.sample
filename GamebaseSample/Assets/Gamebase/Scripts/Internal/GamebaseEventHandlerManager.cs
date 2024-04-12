using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public class GamebaseEventHandlerManager
    {
        private static readonly GamebaseEventHandlerManager instance = new GamebaseEventHandlerManager();

        public static GamebaseEventHandlerManager Instance
        {
            get { return instance; }
        }

        private HashSet<GamebaseCallback.DataDelegate<GamebaseResponse.Event.GamebaseEventMessage>> eventHandlerSet = new HashSet<GamebaseCallback.DataDelegate<GamebaseResponse.Event.GamebaseEventMessage>>();

        public int Handle
        {
            get;
            internal set;
        }

        private GamebaseEventHandlerManager()
        {
            Handle = GamebaseCallbackHandler.RegisterCallback(new GamebaseCallback.DataDelegate<GamebaseResponse.Event.GamebaseEventMessage>(OnEventHandler));
        }

        public void AddEventHandler(GamebaseCallback.DataDelegate<GamebaseResponse.Event.GamebaseEventMessage> eventHandler)
        {
            if (eventHandlerSet.Add(eventHandler) == false)
            {
                GamebaseLog.Warn(GamebaseStrings.ADD_EVENT_HANDLER_FAILED, this);
                return;
            }
        }

        public void RemoveEventHandler(GamebaseCallback.DataDelegate<GamebaseResponse.Event.GamebaseEventMessage> eventHandler)
        {
            if (eventHandlerSet.Remove(eventHandler) == false)
            {
                GamebaseLog.Warn(GamebaseStrings.REMOVE_EVENT_HANDLER_FAILED, this);
                return;
            }
        }

        public void RemoveAllEventHandler()
        {
            if (eventHandlerSet == null)
            {
                return;
            }

            eventHandlerSet.Clear();
        }

        public int GetCount()
        {
            if (eventHandlerSet == null)
            {
                return 0;
            }

            return eventHandlerSet.Count;
        }

        public void OnEventHandler(GamebaseResponse.Event.GamebaseEventMessage message)
        {
            GamebaseLog.Debug(string.Format("message:{0}", LitJson.JsonMapper.ToJson(message)), this);

            if (eventHandlerSet != null && eventHandlerSet.Count > 0)
            {
                foreach (var eventHandler in eventHandlerSet)
                {
                    if (eventHandler != null)
                    {
                        eventHandler(message);
                    }
                }
            }
        }
    }
}
