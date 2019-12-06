using UnityEngine;

namespace GamebaseSample
{
    public class TouchEffectAnimation : MonoBehaviour
    {
        [SerializeField]
        private GameObject touchEffect;

        public void OnFinishAnimation()
        {
            Destroy(touchEffect);
        }
    }
}