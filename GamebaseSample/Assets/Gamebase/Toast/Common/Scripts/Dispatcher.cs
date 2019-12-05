using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Toast.Internal
{
    public class Dispatcher : MonoBehaviour
    {
        private const string GameObjectName = "Toast-Dispatcher";
        private static Dispatcher _instance;

        private Thread _mainThread;
        private readonly Queue<Action> _queue = new Queue<Action>();

        public static Dispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    ToastLog.Error("You MUST call Dispatcher::Initialize" +
                                   " before accessing Dispatcher");
                }

                return _instance;
            }
        }

        /// <summary>
        /// Initialize the dispatcher. This method MUST be called in main thread.
        /// </summary>
        internal static void Initialize()
        {
            // Singleton 을 활용해서 Lazy-initialization 을 할 경우,
            // 다른 쓰레드에서 유니티 API가 호출될 수 있으므로 명시적으로 초기화를 호출하도록 만듬
            if (_instance == null)
            {
                _instance = FindObjectOfType<Dispatcher>();
                if (!_instance)
                {
                    var container = GameObject.Find(Constants.SdkPluginObjectName);
                    if (container == null)
                    {
                        container = new GameObject(Constants.SdkPluginObjectName);
                    }

                    _instance = container.AddComponent<Dispatcher>();
                    DontDestroyOnLoad(_instance);
                }
            }
        }

        internal static bool IsInitialize()
        {
            return _instance != null;
        }

        void Awake()
        {
            _mainThread = Thread.CurrentThread;
        }

        void Update()
        {
            lock (_queue)
            {
                if (_queue.Count <= 0)
                {
                    return;
                }

                while (_queue.Count > 0)
                {
                    var action = _queue.Dequeue();
                    action();
                }
            }
        }

        /// <summary>
        /// Post a action that MUST be called in main thread.
        /// If a current thread is main thread, call a action immediately.
        /// </summary>
        /// <param name="action"></param>
        public void Post(Action action)
        {
            if (action == null) return;

            if (_mainThread == Thread.CurrentThread)
            {
                action();
            }
            else
            {
                lock (_queue)
                {
                    _queue.Enqueue(action);
                }
            }
        }
    }
}