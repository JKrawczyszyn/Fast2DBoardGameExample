using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Controllers
{
    public class SpawnController
    {
        private BoardPosition endPosition;
        private BoardPosition currentPosition;
        private BoardPosition direction;

        public void Reset()
        {
            endPosition = BoardPosition.Up;
            currentPosition = BoardPosition.Up;
            direction = BoardPosition.Right;
        }

        public BoardPosition FindNextFreeSpawnPosition(BoardModel model, BoardPosition startPosition)
        {
            foreach (BoardPosition relativePosition in GetPositions(model.Width * model.Height))
            {
                BoardPosition position = startPosition + relativePosition;

                if (model.IsPositionValid(position) && model.GetField(position) == FieldType.Open
                    && model.GetItem(position) == ItemType.None)
                    return position;
            }

            return default;
        }

        private IEnumerable<BoardPosition> GetPositions(int maxElements)
        {
            yield return currentPosition;

            for (var i = 0; i < maxElements; i++)
            {
                if (Mathf.Abs(currentPosition.X) == Mathf.Abs(currentPosition.Y))
                    direction = direction.RotatedClockwise();

                currentPosition += direction;

                if (currentPosition == endPosition)
                {
                    currentPosition += BoardPosition.Up;
                    endPosition += BoardPosition.Up;
                }

                yield return currentPosition;
            }
        }
    }
}
