using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public class GamebaseObserverManager
    {
        private static readonly GamebaseObserverManager instance = new GamebaseObserverManager();

        public static GamebaseObserverManager Instance
        {
            get { return instance; }
        }

        private HashSet<GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage>> observerSet = new HashSet<GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage>>();

        public int Handle
        {
            get;
            internal set;
        }

        private GamebaseObserverManager()
        {
            Handle = GamebaseCallbackHandler.RegisterCallback(new GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage>(OnObserverEvent));            
        }

        public void AddObserver(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage> observer)
        {
            if (observerSet.Add(observer) == false)
            {
                GamebaseLog.Warn(GamebaseStrings.ADD_OBSERVER_FAILED, this);
                return;
            }
        }

        public void RemoveObserver(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage> observer)
        {
            if (observerSet.Remove(observer) == false)
            {
                GamebaseLog.Warn(GamebaseStrings.REMOVE_OBSERVER_FAILED, this);
                return;
            }
        }

        public void RemoveAllObserver()
        {
            if (observerSet == null)
            {
                return;
            }

            if (observerSet.Count == 0)
            {
                return;
            }

            observerSet.Clear();
        }

        public int GetCount()
        {
            if(observerSet == null)
            {
                return 0;
            }

            return observerSet.Count;
        }

        public void OnObserverEvent(GamebaseResponse.SDK.ObserverMessage message)
        {
            if (observerSet != null && observerSet.Count > 0)
            {
                foreach (var observer in observerSet)
                {
                    if (observer != null)
                    {
                        observer(message);
                    }
                }
            }
        }        
    }
}