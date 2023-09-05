using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField]
        private Slider scaleSlider;

        private InputView inputView;

        private void Awake()
        {
            Inject();
        }

        private void Inject()
        {
            inputView = DiManager.Instance.Resolve<InputView>();
        }

        private void Start()
        {
            InitializeSlider();
        }

        private void InitializeSlider()
        {
            scaleSlider.minValue = inputView.ScaleMin;
            scaleSlider.maxValue = inputView.ScaleMax;
            scaleSlider.value = inputView.ScaleMax;

            scaleSlider.onValueChanged.AddListener(ScaleSliderValueChanged);

            ScaleSliderValueChanged(inputView.ScaleMax);
        }

        private void ScaleSliderValueChanged(float value)
        {
            inputView.SetScale(value);
        }

        private void OnDestroy()
        {
            scaleSlider.onValueChanged.RemoveListener(ScaleSliderValueChanged);
        }
    }
}
