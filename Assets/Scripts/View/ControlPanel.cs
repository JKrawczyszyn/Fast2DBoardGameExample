using Controller;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace View
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField]
        private Slider scaleSlider;

        [SerializeField]
        private PressButton spawnButton;

        private InputView inputView;
        private BoardController boardController;

        private void Awake()
        {
            Inject();
        }

        private void Inject()
        {
            inputView = DiManager.Instance.Resolve<InputView>();
            boardController = DiManager.Instance.Resolve<BoardController>();
        }

        private void Start()
        {
            InitializeSlider();
            InitializeSpawnButton();
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

        private void InitializeSpawnButton()
        {
            spawnButton.OnPress += boardController.ResetSpawner;
        }

        private void Update()
        {
            if (spawnButton.Pressed)
                boardController.SpawnItem();
        }

        private void OnDestroy()
        {
            scaleSlider.onValueChanged.RemoveListener(ScaleSliderValueChanged);
            spawnButton.OnPress -= boardController.ResetSpawner;
        }
    }
}
