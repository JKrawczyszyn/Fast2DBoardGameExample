using Model;
using UnityEngine;
using Utilities;

namespace View
{
    public class CoordConverter
    {
        private readonly Camera camera;
        private readonly int width;
        private readonly int height;

        public CoordConverter(Camera camera, int width, int height)
        {
            this.camera = camera;
            this.width = width;
            this.height = height;
        }

        public Vector3 BoardToWorld(in BoardPosition position)
        {
            float x = position.X - ((width - 1) / 2f);
            float y = position.Y - ((height - 1) / 2f);

            return new Vector3(x, y, 0f);
        }

        public BoardPosition WorldToBoard(Vector2 position)
        {
            float x = position.x + ((width - 1) / 2f);
            float y = position.y + ((height - 1) / 2f);

            return new BoardPosition(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        }

        public void MoveByScreenDelta(Transform transform, Vector2 delta)
        {
            Vector3 screenPosition = camera.WorldToScreenPoint(transform.position);

            screenPosition += (Vector3)delta;

            transform.localPosition = camera.ScreenToWorldPoint(screenPosition);
        }

        public void ClampToBoard(Transform transform)
        {
            Vector3 min = BoardToWorld(BoardPosition.Zero);
            Vector3 max = BoardToWorld(new BoardPosition(width - 1, height - 1));

            var position = transform.position;

            float x = position.x.Clamp(min.x, max.x);
            float y = position.y.Clamp(min.y, max.y);

            transform.localPosition = new Vector3(x, y, 0f);
        }
    }
}
