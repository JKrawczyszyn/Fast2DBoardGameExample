using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class InputView : MonoBehaviour, IDragHandler
{
    private BoardConfig boardConfig;
    private Camera camera;

    public float ScaleMin => 1f;
    public float ScaleMax => 0.5f * Mathf.Max(boardConfig.Width, boardConfig.Height) / Mathf.Min(camera.aspect, 1f);

    private void Awake()
    {
        Inject();
    }

    private void Inject()
    {
        boardConfig = DiManager.Instance.Resolve<BoardConfig>();
        camera = DiManager.Instance.Resolve<Camera>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 screenPosition = camera.WorldToScreenPoint(camera.transform.position);

        screenPosition -= (Vector3)eventData.delta;

        camera.transform.position = camera.ScreenToWorldPoint(screenPosition);
    }

    public void SetScale(float value)
    {
        camera.orthographicSize = value;
    }
}
