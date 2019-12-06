using UnityEngine;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class VersionWaterMark : MonoBehaviour
    {
        private void Start()
        {
            Text text = GetComponent<Text>();
            text.text = SampleVersion.VERSION;
        }
    }
}