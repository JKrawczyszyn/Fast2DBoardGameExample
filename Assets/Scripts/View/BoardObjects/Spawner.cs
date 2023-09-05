using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    [RequireComponent(typeof(Collider2D))]
    public class Spawner : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public event System.Action<Vector2> OnDragEnded;

        private CoordConverter coordConverter;

        public void Initialize(CoordConverter coordConverter)
        {
            this.coordConverter = coordConverter;
        }

        public void OnDrag(PointerEventData eventData)
        {
            coordConverter.MoveByScreenDelta(transform, eventData.delta);
            coordConverter.ClampToBoard(transform);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDragEnded?.Invoke(transform.position);
        }
    }
}
