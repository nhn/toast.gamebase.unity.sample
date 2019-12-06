namespace UnityEngine.UI
{
    public class Progressbar : Scrollbar
    {
        private const float PERCENT_MIN_VALUE = 0f;
        private const float PERCENT_MAX_VALUE = 100f;


        public float Percentage
        {
            get { return size * PERCENT_MAX_VALUE; }
            set
            {
                value /= PERCENT_MAX_VALUE;
                size = Mathf.Clamp(value, PERCENT_MIN_VALUE, PERCENT_MAX_VALUE);
            }
        }
    }
}
