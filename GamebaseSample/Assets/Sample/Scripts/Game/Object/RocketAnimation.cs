using System.Collections;
using UnityEngine;

namespace GamebaseSample
{
    public class RocketAnimation : MonoBehaviour
    {
        public enum AnimationSpeed
        {
            SPEED_FAST,
            SPEED_SLOW,
        }

        private readonly int[] animationOrder = { 0, 1, 2, 3, 4, 3, 2, 1 };

        [SerializeField]
        private GameObject[] sprite;
        private float animationIndex = 0f;
        private AnimationSpeed animationSpeed = AnimationSpeed.SPEED_SLOW;

        private void Start()
        {
            StartCoroutine(StartAnimation());
        }

        public void SetAnimationSpeed(AnimationSpeed type)
        {
            animationSpeed = type;
        }

        private IEnumerator StartAnimation()
        {
            while (true)
            {
                yield return null;
                sprite[animationOrder[(int)animationIndex]].SetActive(false);
                switch (animationSpeed)
                {
                    case AnimationSpeed.SPEED_SLOW:
                        animationIndex += Time.deltaTime * 10;
                        break;
                    case AnimationSpeed.SPEED_FAST:
                        animationIndex += Time.deltaTime * 20;
                        break;
                }

                animationIndex = animationIndex % (animationOrder.Length - 1);
                sprite[animationOrder[(int)animationIndex]].SetActive(true);
            }
        }
    }
}