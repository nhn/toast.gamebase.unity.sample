using System.Collections.Generic;
using UnityEngine;

namespace GamebaseSample
{
    public class SpawnAgent : MonoBehaviour
    {
        private const float SPAWN_INTERVAL = 0.25f;
        private const int SPAWN_ENEMY_POOL_COUNT = 20;

        private readonly ObjectPool<EnemyRocket> enemyPool = new ObjectPool<EnemyRocket>();
        private readonly List<EnemyRocket> enemies = new List<EnemyRocket>();

        private MapData mapData;

        private float spawnTimer;

        private RectTransform rectTransform;

        public List<EnemyRocket> Enemies
        {
            get { return enemies; }
        }

        public void Init(MapData map)
        {
            Stop();
            mapData = map;
        }

        public void Play()
        {
            enabled = true;
        }

        public void Stop()
        {
            enabled = false;
        }

        public void Despawn(EnemyRocket enemy)
        {
            enemies.Remove(enemy);
            enemyPool.Despawn(enemy);
        }

        private void Awake()
        {
            rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = Vector3.zero;

            enemyPool.Allocate(Resources.Load<EnemyRocket>("Prefabs/Enemy"), transform, SPAWN_ENEMY_POOL_COUNT);
            spawnTimer = 0;
        }

        private void Update()
        {
            if (enemyPool.IsEmpty == true)
            {
                return;
            }

            spawnTimer += Time.deltaTime;

            if (spawnTimer >= SPAWN_INTERVAL)
            {
                spawnTimer = 0;

                var pos = GenerateSpawnPoint();

                var enemy = enemyPool.Spawn(pos, Quaternion.identity);
                if (enemy != null)
                {
                    enemy.transform.localPosition = pos;
                    enemy.transform.localScale = Vector3.one;
                    enemies.Add(enemy);
                }
            }
        }

        private Vector2 GenerateSpawnPoint()
        {
            return new Vector2(
                Random.Range(mapData.enemySpawnRect.xMin, mapData.enemySpawnRect.xMax),
                Random.Range(mapData.enemySpawnRect.yMin, mapData.enemySpawnRect.yMax));
        }
    }
}