using UnityEngine;

namespace GamebaseSample
{
    public class TouchEffectAnimation : MonoBehaviour
    {
        [SerializeField]
        private GameObject touchEffect = null;

        public void OnFinishAnimation()
        {
            Destroy(touchEffect);
        }
    }
}