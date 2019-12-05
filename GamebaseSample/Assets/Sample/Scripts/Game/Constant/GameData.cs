using UnityEngine;

namespace GamebaseSample
{
    public class MapData
    {
        public float spawnTimeInterval = 1;

        public Vector3 playerStartPos;
        public Rect enemySpawnRect;
    }

    public class BulletData
    {
        public float radius;
        public float speed;
        public float acceleration;
    }

    public static class GameUtil
    {
        public static bool IntersectCircleCircle(Vector3 circle1Center, float circle1Radius, Vector3 circle2Center, float circle2Radius)
        {
            return circle1Radius + circle2Radius > Vector3.Distance(circle1Center, circle2Center);
        }
    }
}