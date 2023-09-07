using TMPro;
using UnityEngine;
using View.Config;

namespace View
{
    public class FpsView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        private ViewConfig viewConfig;

        private float smoothedDeltaTime;

        private void Update()
        {
            smoothedDeltaTime += (Time.deltaTime - smoothedDeltaTime) * 0.1f;
            var fps = 1f / smoothedDeltaTime;

            text.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }
}
