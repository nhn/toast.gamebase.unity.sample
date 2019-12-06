using UnityEngine;

namespace GamebaseSample
{
    public class BulletAgent
    {
        private const int BULLET_POOL_COUNT = 30;

        private readonly GameObject groupObject;
        private readonly ObjectPool<Bullet> bulletPool;

        public BulletAgent(Transform parent)
        {
            groupObject = new GameObject("Bullets");
            groupObject.transform.parent = parent;
            groupObject.transform.localScale = Vector3.one;
            groupObject.transform.localPosition = Vector3.zero;
            groupObject.AddComponent<RectTransform>();

            bulletPool = new ObjectPool<Bullet>();
            bulletPool.Allocate(Resources.Load<Bullet>("Prefabs/Bullet"), groupObject.transform, BULLET_POOL_COUNT);
        }

        public Bullet Spawn(BulletData data, Vector3 startPos)
        {
            var bullet = bulletPool.Spawn(startPos, Quaternion.identity);
            if (bullet == null)
            {
                return null;
            }

            bullet.transform.localScale = Vector3.one;
            bullet.Load(this, data);

            return bullet;
        }

        public void Despawn(Bullet bullet)
        {
            bulletPool.Despawn(bullet);
        }
    }
}