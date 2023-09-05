using Model;
using UnityEngine;

namespace Controller
{
    public static class BoardHelpers
    {
        public static BoardPosition GetMiddle(int width, int height) => new(width / 2, height / 2);

        public static BoardPosition GetClosestOpen(BoardModel model, BoardPosition position)
        {
            BoardPosition closest = default;
            int closestDistance = Mathf.Max(model.Width, model.Height);

            for (var x = position.X - closestDistance; x <= position.X + closestDistance; x++)
            {
                for (var y = position.Y - closestDistance; y <= position.Y + closestDistance; y++)
                {
                    if (!x.Between(0, model.Width - 1) || !y.Between(0, model.Height - 1))
                        continue;

                    BoardPosition current = new(x, y);

                    if (model.GetField(current) != FieldType.Open)
                        continue;

                    int distance = Mathf.Abs(position.X - x) + Mathf.Abs(position.Y - y);

                    if (distance < closestDistance)
                    {
                        closest = current;
                        closestDistance = distance;
                    }

                    if (closestDistance == 0)
                        return closest;
                }
            }

            return closest;
        }
    }
}
