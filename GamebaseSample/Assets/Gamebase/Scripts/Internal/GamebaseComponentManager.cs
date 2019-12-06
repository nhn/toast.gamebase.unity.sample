using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public class GamebaseComponentManager
    {
        public static T AddComponent<T>(GamebaseGameObjectManager.GameObjectType gameObjectType) where T : Component
        {
            var gameObject = GamebaseGameObjectManager.GetGameObject(gameObjectType);

            var component = gameObject.GetComponent<T>();

            if (null != component)
            {
                return component;
            }

            return gameObject.AddComponent<T>();
        }

#pragma warning disable 0108
        public static T GetComponent<T>(GamebaseGameObjectManager.GameObjectType gameObjectType)
        {
            if (false == GamebaseGameObjectManager.ContainsGameObject(gameObjectType))
            {
                return default(T);
            }

            var gameObject = GamebaseGameObjectManager.GetGameObject(gameObjectType);

            return gameObject.GetComponent<T>();
        }
    }
}