using UnityEngine;

namespace GamebaseSample
{
    public class IngameAgent
    {
        private const float LIMIT_TIME = 20f;

        private readonly Ingame ingame;
        private readonly BulletAgent bulletAgent;

        private MapData mapData;

        private PlayerRocket player;
        private SpawnAgent spawnAgent;

        private float playingTime;

        public int GameScore { get; private set; }

        public string PlayingTimerString
        {
            get
            {
                float amountTime = LIMIT_TIME - playingTime;

                float minutes = Mathf.Max(0, Mathf.Floor(amountTime / 60));
                float seconds = Mathf.RoundToInt(amountTime % 60);

                return string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }

        public BulletAgent BulletAgent
        {
            get { return bulletAgent; }
        }

        public SpawnAgent SpawnAgent
        {
            get { return spawnAgent; }
        }

        public Rect AreaRect
        {
            get { return ingame.PlayingAreaObject.GetComponent<RectTransform>().rect; }
        }


        public IngameAgent(Ingame ingame)
        {
            this.ingame = ingame;
            this.bulletAgent = new BulletAgent(ingame.transform);
        }

        public void Start()
        {
            GameScore = 0;
            playingTime = 0;

            mapData = new MapData()
            {
                playerStartPos = ingame.StartPointObject.transform.position,
                enemySpawnRect = ingame.SpawnAreaObject.GetComponent<RectTransform>().rect
            };

            GameObject spawnAgentObject = new GameObject("Enemies");
            spawnAgentObject.transform.parent = ingame.transform;
            spawnAgent = spawnAgentObject.AddComponent<SpawnAgent>();
            spawnAgent.Init(mapData);

            spawnAgent.Play();
        }

        public void Update()
        {
            playingTime += Time.deltaTime;

            if (LIMIT_TIME - playingTime <= 0)
            {
                Finish();
            }
        }

        public void Finish()
        {
            Pause();

            DataManager.User.AddExp(GameScore);
            ingame.PrintResult();
        }

        public void Continue()
        {
            Time.timeScale = 1f;
        }

        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Hit(EnemyRocket target)
        {
            spawnAgent.Despawn(target);
            GameScore++;
        }
    }
}