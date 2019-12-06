using System.Collections;
using UnityEngine;

namespace GamebaseSample
{
    public class LoadingPopup : MonoBehaviour
    {
        public GameObject rotationImage = null;

        private Hashtable hash;

        private void Awake()
        {
            hash = iTween.Hash();
            hash.Add("z", -1.0f);
            hash.Add("time", 2f);
            hash.Add("easetype", "easeInOutCirc");
            hash.Add("looptype", iTween.LoopType.loop);
        }

        private void OnEnable()
        {
            iTween.RotateBy(rotationImage, hash);
        }

        private void OnDisable()
        {
            iTween.Stop(gameObject);
        }
    }
}