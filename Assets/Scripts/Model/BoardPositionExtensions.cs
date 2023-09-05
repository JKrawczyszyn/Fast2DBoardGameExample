using System.Collections.Generic;

namespace Model
{
    public static class BoardPositionExtensions
    {
        public static IEnumerable<BoardPosition> GetNeighbors(this BoardPosition startPosition)
        {
            yield return startPosition + BoardPosition.Up;
            yield return startPosition + BoardPosition.Right;
            yield return startPosition + BoardPosition.Down;
            yield return startPosition + BoardPosition.Left;
        }

        public static bool IsNeighborOf(this in BoardPosition p1, in BoardPosition p2)
        {
            if (p1 + BoardPosition.Up == p2)
                return true;

            if (p1 + BoardPosition.Right == p2)
                return true;

            if (p1 + BoardPosition.Down == p2)
                return true;

            if (p1 + BoardPosition.Left == p2)
                return true;

            return false;
        }
    }
}
