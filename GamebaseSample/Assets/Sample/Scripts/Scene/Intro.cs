using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class Intro : MonoBehaviour
    {
        [SerializeField]
        private Image bi = null;
        [SerializeField]
        private Image poweredBy = null;

        private GameObject biTweenObject = null;
        private GameObject poweredByTweenObject = null;

        private void Start()
        {
            if (bi == null)
            {
                Debug.LogWarning("BI image not found.");
                return;
            }

            biTweenObject = bi.gameObject;
            poweredByTweenObject = poweredBy.gameObject;
            ShowBI();
        }

        private void ShowBI()
        {
            BiTweenAlpha(biTweenObject, 0f, 1f, 1f, "Delay");
        }

        private void Delay()
        {
            BiTweenAlpha(biTweenObject, 1f, 1f, 3f, "HideBI");
            PoweredByTweenAlpha(poweredByTweenObject, 0f, 1f, 1f, string.Empty);
        }

        private void HideBI()
        {
            BiTweenAlpha(biTweenObject, 1f, 0f, 1f, "LoadLoginScene");
            PoweredByTweenAlpha(poweredByTweenObject, 1f, 0f, 1f, string.Empty);
        }

        private void LoadLoginScene()
        {
            SceneManager.LoadSceneAsync("login");
        }

        private void BiTweenAlpha(GameObject target, float from, float to, float time, string completeCallback)
        {
            TweenAlpha(target, from, to, time, "BiUpdateColor", completeCallback);
        }
        private void PoweredByTweenAlpha(GameObject target, float from, float to, float time, string completeCallback)
        {
            TweenAlpha(target, from, to, time, "PoweredByUpdateColor", completeCallback);
        }

        private void BiUpdateColor(float value)
        {
            Color color = bi.color;
            color.a = value;
            bi.color = color;
        }

        private void PoweredByUpdateColor(float value)
        {
            Color color = bi.color;
            color.a = value;
            poweredBy.color = color;
        }

        private void TweenAlpha(GameObject target, float from, float to, float time, string updateCallback, string completeCallback)
        {
            iTween.ValueTo(target, iTween.Hash(
                "from", from,
                "to", to,
                "time", time,
                "onupdatetarget", gameObject,
                "onupdate", updateCallback,
                "oncomplete", completeCallback,
                "oncompletetarget", gameObject));
        }
    }
}