using UnityEngine;

namespace GamebaseSample
{
    public abstract class Rocket : MonoBehaviour
    {
        protected float MoveSpeed { get; set; }

        public float Radius { get; protected set; }

        protected virtual void Awake()
        {
        }

        protected virtual void Update()
        {
        }
    }
}