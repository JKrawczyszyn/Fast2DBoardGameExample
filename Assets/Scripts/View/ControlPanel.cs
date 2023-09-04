using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    [SerializeField]
    private Slider scaleSlider;

    private BoardConfig boardConfig;
    private Camera camera;

    private float minScale, maxScale;

    private void Start()
    {
        Inject();

        SetScaleBoundaries();
        InitializeSlider();
    }

    private void Inject()
    {
        boardConfig = DiManager.Instance.Resolve<BoardConfig>();
        camera = DiManager.Instance.Resolve<Camera>();
    }

    private void SetScaleBoundaries()
    {
        minScale = 1f;
        maxScale = Mathf.Max(boardConfig.Width, boardConfig.Height) / 2f / Mathf.Min(camera.aspect, 1f);
    }

    private void InitializeSlider()
    {
        scaleSlider.minValue = minScale;
        scaleSlider.maxValue = maxScale;
        scaleSlider.value = maxScale;

        scaleSlider.onValueChanged.AddListener(ScaleSliderValueChanged);

        ScaleSliderValueChanged(maxScale);
    }

    private void ScaleSliderValueChanged(float value)
    {
        camera.orthographicSize = value;
    }

    private void OnDestroy()
    {
        scaleSlider.onValueChanged.RemoveListener(ScaleSliderValueChanged);
    }
}
