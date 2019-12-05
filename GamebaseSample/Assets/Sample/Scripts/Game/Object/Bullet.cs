using UnityEngine;

namespace GamebaseSample
{
    public class Bullet : MonoBehaviour
    {
        private BulletAgent agent;
        private BulletData data;
        private Vector3 dir = Vector3.up;

        private Rect areaRect;

        public void Load(BulletAgent bulletAgent, BulletData bulletData)
        {
            agent = bulletAgent;
            data = bulletData;

            areaRect = Ingame.Agent.AreaRect;
        }

        private void Update()
        {
            if (data == null)
            {
                return;
            }

            transform.Translate(data.speed * Time.deltaTime * dir);

            if (areaRect.Contains(transform.localPosition) == false)
            {
                agent.Despawn(this);
            }

            var enemies = Ingame.Agent.SpawnAgent.Enemies;

            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyRocket enemy = enemies[i];
                if (GameUtil.IntersectCircleCircle(transform.localPosition, data.radius, enemy.transform.localPosition, enemy.Radius) == true)
                {
                    Ingame.Agent.Hit(enemies[i]);
                    agent.Despawn(this);
                    break;
                }
            }
        }
    }
}