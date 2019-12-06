using UnityEngine;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class SpriteScrolling : MonoBehaviour
    {
        public float Speed = 0.5f;

        private RawImage rawImage;

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
        }

        private void LateUpdate()
        {
            Rect uvRect = rawImage.uvRect;
            uvRect.y += (Speed * Time.deltaTime);
            rawImage.uvRect = uvRect;
        }
    }
}