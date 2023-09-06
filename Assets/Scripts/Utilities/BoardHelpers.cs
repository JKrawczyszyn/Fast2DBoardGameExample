using Model;
using UnityEngine;
using View;

namespace Utilities
{
    public static class BoardHelpers
    {
        public static BoardPosition GetMiddle(int width, int height) => new(width / 2, height / 2);

        public static BoardPosition GetClosestOpen(BoardModel model, BoardPosition position,
            params ItemType[] types)
        {
            BoardPosition closest = default;
            var closestDistance = int.MaxValue;

            for (var x = 0; x < model.Width; x++)
            {
                for (var y = 0; y < model.Height; y++)
                {
                    BoardPosition current = new(x, y);

                    if (!model.IsPositionOpen(current, types))
                        continue;

                    int distance = GetDistance(position, current);

                    if (distance >= closestDistance)
                        continue;

                    closest = current;
                    closestDistance = distance;
                }
            }

            return closest;
        }

        private static int GetDistance(in BoardPosition position1, in BoardPosition position2)
        {
            int x = Mathf.Abs(position1.X - position2.X);
            int y = Mathf.Abs(position1.Y - position2.Y);

            return x + y;
        }

        public static BoardPosition GetClosestOpen(BoardModel model, CoordConverter coordConverter,
            Vector2 position, params ItemType[] types)
        {
            BoardPosition closest = default;
            var closestDistance = float.MaxValue;

            for (var x = 0; x < model.Width; x++)
            {
                for (var y = 0; y < model.Height; y++)
                {
                    BoardPosition current = new(x, y);

                    if (!model.IsPositionOpen(current, types))
                        continue;

                    Vector3 worldPosition = coordConverter.BoardToWorld(current);
                    float distance = Vector2.Distance(position, worldPosition);

                    if (distance >= closestDistance)
                        continue;

                    closest = current;
                    closestDistance = distance;
                }
            }

            return closest;
        }
    }
}
