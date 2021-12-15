using System.Collections.Generic;
using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public class GamebaseGameObjectManager
    {

        public enum GameObjectType
        {
            CORE_TYPE,
            LAUNCHING_TYPE,
            HEARTBEAT_TYPE,
            WEBSOCKET_TYPE,
            DISPLAY_LANGUAGE_TYPE,
            INDICATOR_REPORT_TYPE,
            PLUGIN_TYPE,
            WATER_MARK_TYPE,
            STRING_LOADER,
            INTROSPECT_TYPE,
        }

        static private Dictionary<GameObjectType, GameObject> gameObjectDictionary = new Dictionary<GameObjectType, GameObject>();


        public static bool ContainsGameObject(GameObjectType gameObjectType)
        {
            return gameObjectDictionary.ContainsKey(gameObjectType);
        }

        public static GameObject GetGameObject(GameObjectType gameObjectType)
        {
            if (false == ContainsGameObject(gameObjectType))
            {
                return CreateGameObject(gameObjectType);
            }

            return gameObjectDictionary[gameObjectType].gameObject;
        }

        private static GameObject CreateGameObject(GameObjectType gameObjectType)
        {
            if (true == gameObjectDictionary.ContainsKey(gameObjectType))
            {
                return gameObjectDictionary[gameObjectType];
            }

            GameObject gameObject = new GameObject();
            gameObject.name = gameObjectType.ToString();
            GameObject.DontDestroyOnLoad(gameObject);
            gameObjectDictionary.Add(gameObjectType, gameObject);

            return gameObject;
        }
    }
}