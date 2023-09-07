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

        [SerializeField]
        private Button clearButton;

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
            InitializeButtons();
        }

        private void InitializeSlider()
        {
            scaleSlider.minValue = inputView.ScaleMin;
            scaleSlider.maxValue = inputView.ScaleMax;
            scaleSlider.value = inputView.ScaleMax;
            scaleSlider.onValueChanged.AddListener(inputView.SetScale);

            inputView.SetScale(inputView.ScaleMax);
        }

        private void InitializeButtons()
        {
            spawnButton.OnPress += boardController.ResetSpawner;
            clearButton.onClick.AddListener(boardController.ClearAdjacent);
        }

        private void Update()
        {
            if (spawnButton.Pressed)
                boardController.SpawnItem();
        }

        private void OnDestroy()
        {
            scaleSlider.onValueChanged.RemoveListener(inputView.SetScale);
            spawnButton.OnPress -= boardController.ResetSpawner;
            clearButton.onClick.RemoveListener(boardController.ClearAdjacent);
        }
    }
}
