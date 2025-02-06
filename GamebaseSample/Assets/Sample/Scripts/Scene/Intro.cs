using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Toast.Gamebase;

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
        
        private bool isGamebaseInitialized = false;

        private enum State
        {
            None,
            ShowBI,
            FinishBI,
            Loading,
        }
        private State state = State.None;

        private void Start()
        {
            if (bi == null)
            {
                Debug.LogWarning("BI image not found.");
                return;
            }

            biTweenObject = bi.gameObject;
            poweredByTweenObject = poweredBy.gameObject;

            InitializeGamebase();
            
            ShowBI();
        }

        private void InitializeGamebase()
        {
            Gamebase.SetDebugMode(true);
            
            var configuration = new GamebaseRequest.GamebaseConfiguration();
            configuration.appID = "6ypq5kwa";
            configuration.appVersion = "1.0.0";
            configuration.displayLanguageCode = GamebaseDisplayLanguageCode.English;
#if UNITY_ANDROID
            configuration.storeCode = GamebaseStoreCode.GOOGLE;
#elif UNITY_IOS
            configuration.storeCode = GamebaseStoreCode.APPSTORE;
#elif UNITY_WEBGL
            configuration.storeCode = GamebaseStoreCode.WEBGL;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            configuration.storeCode = GamebaseStoreCode.MACOS;
#else
            configuration.storeCode = GamebaseStoreCode.WINDOWS;
#endif
            Gamebase.Initialize(configuration, (data, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    isGamebaseInitialized = true;
                }
            });
        }

        private void Update()
        {
            if (isGamebaseInitialized && state == State.FinishBI)
            {
                LoadLoginScene();
                state = State.Loading;
            }
        }
        
        private void LoadLoginScene()
        {
            SceneManager.LoadSceneAsync("login");
        }

        private void ShowBI()
        {
            state = State.ShowBI;
            BiTweenAlpha(biTweenObject, 0f, 1f, 1f, "Delay");
        }

        private void Delay()
        {
            BiTweenAlpha(biTweenObject, 1f, 1f, 3f, "HideBI");
            PoweredByTweenAlpha(poweredByTweenObject, 0f, 1f, 1f, string.Empty);
        }

        private void HideBI()
        {
            BiTweenAlpha(biTweenObject, 1f, 0f, 1f, "FinishBI");
            PoweredByTweenAlpha(poweredByTweenObject, 1f, 0f, 1f, string.Empty);
        }

        private void FinishBI()
        {
            state = State.FinishBI;
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