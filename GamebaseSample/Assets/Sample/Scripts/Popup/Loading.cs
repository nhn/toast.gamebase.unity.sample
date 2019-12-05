using UnityEngine;

namespace GamebaseSample
{
    public class Loading : MonoBehaviour
    {
        public static readonly string OBJECT_NAME = "GamebaseLoading";

        private static Loading instance = null;

        public static Loading GetInstance()
        {
            if (instance == null)
            {
                instance = new GameObject(OBJECT_NAME).AddComponent<Loading>();
            }

            return instance;
        }

        private GameObject loadingPrefeb;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void ShowLoading(GameObject parent)
        {
            if (loadingPrefeb == null)
            {
                loadingPrefeb = PopupManager.ShowPopup(parent, "Loading");
            }
            else
            {
                loadingPrefeb.transform.SetParent(parent.transform, false);
            }

            loadingPrefeb.SetActive(true);
        }

        public void HideLoading()
        {
            loadingPrefeb.SetActive(false);
        }
    }
}