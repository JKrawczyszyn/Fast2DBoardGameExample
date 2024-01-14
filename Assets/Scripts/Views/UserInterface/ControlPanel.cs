using Controllers;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Views
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
            inputView = ServiceLocator.Instance.Resolve<InputView>();
            boardController = ServiceLocator.Instance.Resolve<BoardController>();
        }

        private void Start()
        {
            InitializeSlider();
            InitializeButtons();
        }

        private void InitializeSlider()
        {
            float min = inputView.ScaleMin;
            float max = Mathf.Sqrt(Mathf.Sqrt(Mathf.Sqrt(inputView.ScaleMax)));
            
            scaleSlider.minValue = min;
            scaleSlider.maxValue = max;
            scaleSlider.value = max;
            scaleSlider.onValueChanged.AddListener(SetScale);

            SetScale(max);
        }

        private void SetScale(float scale)
        {
            inputView.SetScale(Mathf.Pow(scale, 8));
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
            scaleSlider.onValueChanged.RemoveListener(SetScale);
            spawnButton.OnPress -= boardController.ResetSpawner;
            clearButton.onClick.RemoveListener(boardController.ClearAdjacent);
        }
    }
}
