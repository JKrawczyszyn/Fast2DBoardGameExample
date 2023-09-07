using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace View
{
    [RequireComponent(typeof(Collider2D))]
    public class InputView : MonoBehaviour, IDragHandler
    {
        [SerializeField]
        private BoxCollider2D collider;

        private BoardConfig boardConfig;
        private Camera camera;
        private CoordConverter coordConverter;

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
            coordConverter = DiManager.Instance.Resolve<CoordConverter>();
        }

        private void Start()
        {
            collider.size = new Vector2(boardConfig.Width, boardConfig.Height);
        }

        public void OnDrag(PointerEventData eventData)
        {
            coordConverter.MoveByScreenDelta(camera.transform, -eventData.delta);
        }

        public void SetScale(float value)
        {
            camera.orthographicSize = value;
        }
    }
}
