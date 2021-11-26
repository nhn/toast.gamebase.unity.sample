using System.Collections.Generic;
using Toast.Gamebase.LitJson;

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
            if (false == observerSet.Add(observer))
            {
                GamebaseLog.Warn(GamebaseStrings.ADD_OBSERVER_FAILED, this);
                return;
            }
        }

        public void RemoveObserver(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage> observer)
        {
            if (false == observerSet.Remove(observer))
            {
                GamebaseLog.Warn(GamebaseStrings.REMOVE_OBSERVER_FAILED, this);
                return;
            }
        }

        public void RemoveAllObserver()
        {
            if (null == observerSet)
            {
                return;
            }

            if (0 == observerSet.Count)
            {
                return;
            }

            observerSet.Clear();
        }

        public int GetCount()
        {
            if(null == observerSet)
            {
                return 0;
            }

            return observerSet.Count;
        }

        public void OnObserverEvent(GamebaseResponse.SDK.ObserverMessage message)
        {
            if (null != observerSet && 0 < observerSet.Count)
            {
                foreach (var observer in observerSet)
                {
                    if (null != observer)
                    {
                        observer(message);
                    }
                }
            }
        }        
    }
}