using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Controllers
{
    public static class ControllerHelpers
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

        public static HashSet<BoardPosition> GetAdjacentPositions(BoardModel model)
        {
            HashSet<BoardPosition> positions = new();

            for (var x = 0; x < model.Width; x++)
            {
                for (var y = 0; y < model.Height; y++)
                {
                    BoardPosition position = new(x, y);

                    if (model.IsPositionBlocked(position))
                        continue;

                    ItemType type = model.GetItem(position);

                    if (type == ItemType.None)
                        continue;

                    bool adjacent = x < model.Width - 1 && Find(model, x + 1, y, type, ref positions);
                    adjacent = adjacent || (y < model.Height - 1 && Find(model, x, y + 1, type, ref positions));

                    if (adjacent)
                        positions.Add(position);
                }
            }

            return positions;
        }

        private static bool Find(BoardModel model, int x, int y, ItemType type, ref HashSet<BoardPosition> positions)
        {
            BoardPosition position = new(x, y);

            if (model.GetItem(position) != type)
                return false;

            positions.Add(position);

            return true;
        }
    }
}
