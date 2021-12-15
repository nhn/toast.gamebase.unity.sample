using UnityEngine;

namespace GamebaseSample
{
    public class PlayerRocket : Rocket
    {
        private const float SHOT_INTERVAL = 0.1f;
        private const float MOVE_CHECK_RANGE = 0.2f;
        private const float REACH_RANGE = 20f;

        [SerializeField] private Transform shotPoint = null;

        private float enableShotTime;
        private Vector2 moveTargetPos;

        private Rect areaRect;
        private Canvas rootCanvas;

        private readonly BulletData bulletData = new BulletData()
        {
            radius = 30,
            speed = 1,
        };

        protected override void Awake()
        {
            base.Awake();

            enableShotTime = Time.realtimeSinceStartup + SHOT_INTERVAL;

            MoveSpeed = 1100f;
            Radius = 60;
            areaRect = Ingame.Agent.AreaRect;
            moveTargetPos = transform.localPosition;

            rootCanvas = GetComponentInParent<Canvas>();
        }

        protected override void Update()
        {
            base.Update();
            CheckInput();

            if (Time.realtimeSinceStartup >= enableShotTime)
            {
                Ingame.Agent.BulletAgent.Spawn(bulletData, shotPoint.position);
                enableShotTime = Time.realtimeSinceStartup + SHOT_INTERVAL;
            }

            var enemies = Ingame.Agent.SpawnAgent.Enemies;

            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyRocket enemy = enemies[i];
                if (GameUtil.IntersectCircleCircle(transform.localPosition, Radius, enemy.transform.localPosition, enemy.Radius) == true)
                {
                    Ingame.Agent.Finish();
                }
            }
        }

        private void CheckInput()
        {
            if (Input.GetMouseButton(0) == true)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rootCanvas.transform as RectTransform, Input.mousePosition, rootCanvas.worldCamera, out moveTargetPos);
            }

            var localPosition = transform.localPosition;

            if (Vector3.Distance(localPosition, moveTargetPos) > MOVE_CHECK_RANGE)
            {
                Vector3 dir = new Vector3(moveTargetPos.x - localPosition.x, moveTargetPos.y - localPosition.y);
                dir.Normalize();

                Vector3 nextMovePos = localPosition + (Time.deltaTime * MoveSpeed * dir);
                if (Vector3.Distance(moveTargetPos, nextMovePos) <= REACH_RANGE)
                {
                    nextMovePos = moveTargetPos;
                }

                if (nextMovePos.x < areaRect.x)
                {
                    nextMovePos.x = areaRect.x;
                }
                if (nextMovePos.x > areaRect.x + areaRect.width)
                {
                    nextMovePos.x = areaRect.x + areaRect.width;
                }
                if (nextMovePos.y < areaRect.y)
                {
                    nextMovePos.y = areaRect.y;
                }
                if (nextMovePos.y > areaRect.y + areaRect.height)
                {
                    nextMovePos.y = areaRect.y + areaRect.height;
                }

                transform.localPosition = nextMovePos;
            }
        }
    }
}