using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public static class GamebaseCoroutineManager
    {
        public static Coroutine StartCoroutine(GamebaseGameObjectManager.GameObjectType gameObjectType, IEnumerator routine)
        {
            GamebaseCoroutineComponent coroutineComponent = GetCoroutineComponent(gameObjectType, true);

            return coroutineComponent.StartCoroutine(routine);
        }

        public static Coroutine StartCoroutine(GamebaseGameObjectManager.GameObjectType gameObjectType, string methodName, [DefaultValue("null")] object value)
        {
            GamebaseCoroutineComponent coroutineComponent = GetCoroutineComponent(gameObjectType, true);

            return coroutineComponent.StartCoroutine(methodName, value);
        }

        public static Coroutine StartCoroutine(GamebaseGameObjectManager.GameObjectType gameObjectType, string methodName)
        {
            GamebaseCoroutineComponent coroutineComponent = GetCoroutineComponent(gameObjectType, true);

            return coroutineComponent.StartCoroutine(methodName);
        }

        public static void StopAllCoroutines(GamebaseGameObjectManager.GameObjectType gameObjectType)
        {
            GamebaseCoroutineComponent coroutineComponent = GetCoroutineComponent(gameObjectType);

            if (null == coroutineComponent)
            {
                return;
            }

            coroutineComponent.StopAllCoroutines();
        }

        public static void StopCoroutine(GamebaseGameObjectManager.GameObjectType gameObjectType, string methodName)
        {
            GamebaseCoroutineComponent coroutineComponent = GetCoroutineComponent(gameObjectType);

            if (null == coroutineComponent)
            {
                return;
            }

            coroutineComponent.StopCoroutine(methodName);
        }

        public static void StopCoroutine(GamebaseGameObjectManager.GameObjectType gameObjectType, IEnumerator routine)
        {
            GamebaseCoroutineComponent coroutineComponent = GetCoroutineComponent(gameObjectType);

            if (null == coroutineComponent)
            {
                return;
            }

            coroutineComponent.StopCoroutine(routine);
        }

        public static void StopCoroutine(GamebaseGameObjectManager.GameObjectType gameObjectType, Coroutine routine)
        {
            GamebaseCoroutineComponent coroutineComponent = GetCoroutineComponent(gameObjectType);

            if (null == coroutineComponent)
            {
                return;
            }

            coroutineComponent.StopCoroutine(routine);
        }

        private static GamebaseCoroutineComponent GetCoroutineComponent(GamebaseGameObjectManager.GameObjectType gameObjectType, bool isMake = false)
        {
            GamebaseCoroutineComponent coroutineComponent = GamebaseComponentManager.GetComponent<GamebaseCoroutineComponent>(gameObjectType);

            if (null == coroutineComponent && true == isMake)
            {
                coroutineComponent = GamebaseComponentManager.AddComponent<GamebaseCoroutineComponent>(gameObjectType);
            }

            return coroutineComponent;
        }
    }
}