using System.Collections.Generic;
using UnityEngine;

namespace GamePlatform.Logger.Internal
{
    public static class GameObjectManager
    {
        private static Dictionary<string, GameObject> gameObjectDictionary = new Dictionary<string, GameObject>();

        public static CoroutineComponent GetCoroutineComponent(string gameObjectType)
        {
            return GetGameObject(gameObjectType).GetComponent<CoroutineComponent>();
        }

        public static void AddCrashLoggerListener(string gameObjectType)
        {
            var gameObject = GetGameObject(gameObjectType);
            var receiver = gameObject.GetComponent<CrashLoggerReceiver>();
            if (receiver == null)
            {
                gameObject.AddComponent<CrashLoggerReceiver>();
            }
        }

        private static GameObject GetGameObject(string gameObjectType)
        {
            if (ContainsGameObject(gameObjectType) == false)
            {
                return CreateGameObject(gameObjectType);
            }

            return gameObjectDictionary[gameObjectType].gameObject;
        }

        private static bool ContainsGameObject(string gameObjectType)
        {
            return gameObjectDictionary.ContainsKey(gameObjectType);
        }

        private static GameObject CreateGameObject(string gameObjectType)
        {
            GameObject gameObject = new GameObject
            {
                name = gameObjectType.ToString()
            };

            gameObject.AddComponent<CoroutineComponent>();

            Object.DontDestroyOnLoad(gameObject);
            gameObjectDictionary.Add(gameObjectType, gameObject);

            return gameObject;
        }
    }
}
