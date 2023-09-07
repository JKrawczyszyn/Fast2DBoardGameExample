using Models;
using UnityEngine;

namespace Views
{
    public static class ViewHelpers
    {
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
