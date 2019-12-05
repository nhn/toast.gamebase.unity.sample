using System.Collections.Generic;
using UnityEngine;

namespace GamebaseSample
{
    public class ObjectPool<T> where T : Component
    {
        private readonly Queue<T> objects = new Queue<T>();
        private Transform parent;

        public bool IsEmpty
        {
            get { return objects.Count == 0; }
        }

        public void Allocate(T prefab, Transform transform, int amount)
        {
            parent = transform;

            for (int i = 0; i < amount; i++)
            {
                T createdObject = UnityEngine.Object.Instantiate(prefab) as T;
                createdObject.name = prefab.name;

                Despawn(createdObject);
            }
        }

        public T Spawn(Vector3 pos, Quaternion rot)
        {
            if (objects.Count == 0)
            {
                return null;
            }

            T spawn = objects.Dequeue();

            spawn.transform.position = pos;
            spawn.transform.rotation = rot;
            spawn.transform.localScale = Vector3.one;
            spawn.gameObject.SetActive(true);

            return spawn;
        }

        public void Despawn(T obj)
        {
            obj.gameObject.SetActive(false);
            if (parent != null)
            {
                obj.GetComponent<RectTransform>().SetParent(parent);
            }

            objects.Enqueue(obj);
        }
    }
}