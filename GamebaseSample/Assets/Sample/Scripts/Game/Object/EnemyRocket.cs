namespace GamebaseSample
{
    public class EnemyRocket : Rocket
    {
        protected override void Awake()
        {
            base.Awake();
            Radius = 45;
        }
    }
}