using System;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;

namespace Controllers
{
    public class BoardAlgorithmService
    {
        private int maxSearchDistance;

        private BoardPosition[] nearestOffsetsLookup;
        private int[] distanceBreakupIndexesLookup;

        public void Initialize(int startMaxSearchDistance)
        {
            Assert.IsTrue(startMaxSearchDistance > 0, "Max search distance must be greater than 0.");

            maxSearchDistance = startMaxSearchDistance;

            GenerateLookups();
        }

        private void GenerateLookups()
        {
            nearestOffsetsLookup = GenerateNearestOffsets();
            distanceBreakupIndexesLookup = GenerateDistanceBreakupIndexes();
        }

        private BoardPosition[] GenerateNearestOffsets()
        {
            List<BoardPosition> nearestPositions = new();

            int maxDistanceSqr = maxSearchDistance * maxSearchDistance;

            for (int x = -maxSearchDistance; x <= maxSearchDistance; x++)
            {
                for (int y = -maxSearchDistance; y <= maxSearchDistance; y++)
                {
                    BoardPosition position = new(x, y);

                    if (position.MagnitudeSqr > maxDistanceSqr)
                        continue;

                    nearestPositions.Add(position);
                }
            }

            nearestPositions.Sort((position1, position2) => position1.MagnitudeSqr.CompareTo(position2.MagnitudeSqr));

            return nearestPositions.ToArray();
        }

        private int[] GenerateDistanceBreakupIndexes()
        {
            List<int> indexes = new();

            var currentDistance = 1;

            for (var i = 0; i < nearestOffsetsLookup.Length; i++)
            {
                BoardPosition position = nearestOffsetsLookup[i];

                if (position.MagnitudeSqr <= currentDistance * currentDistance)
                    continue;

                indexes.Add(i);

                currentDistance++;
            }

            indexes.Add(nearestOffsetsLookup.Length);

            return indexes.ToArray();
        }

        public BoardPosition GetClosestOpen(BoardModel model, Vector2 floatPosition, ItemType[] types)
        {
            while (true)
            {
                Span<BoardPosition> nearestOffsets = nearestOffsetsLookup;

                var startIndex = 0;

                foreach (int endIndex in distanceBreakupIndexesLookup)
                {
                    Span<BoardPosition> searchOffsets = nearestOffsets.Slice(startIndex, endIndex - startIndex);

                    BoardPosition boardPosition = GetClosestOpen(model, floatPosition, types, searchOffsets);

                    if (boardPosition != default)
                        return boardPosition;

                    startIndex = endIndex;
                }

                if (maxSearchDistance >= Mathf.Max(model.Width, model.Height))
                    return default;

                maxSearchDistance = Mathf.Min(maxSearchDistance + 2, model.Width, model.Height);

                GenerateLookups();
            }
        }

        private BoardPosition GetClosestOpen(BoardModel model, Vector2 floatPosition, ItemType[] types,
            Span<BoardPosition> searchOffsets)
        {
            BoardPosition boardPosition = new(Mathf.RoundToInt(floatPosition.x), Mathf.RoundToInt(floatPosition.y));

            BoardPosition nearestPosition = default;
            var nearestDistance = float.MaxValue;

            foreach (BoardPosition offset in searchOffsets)
            {
                BoardPosition current = boardPosition + offset;

                if (!current.X.Between(0, model.Width - 1) || !current.Y.Between(0, model.Height - 1))
                    continue;

                if (model.GetField(current) != FieldType.Open || !model.IsItemOneOfType(current, types))
                    continue;

                float distance = current.DistanceSqrTo(floatPosition);

                if (distance >= nearestDistance)
                    continue;

                nearestDistance = distance;
                nearestPosition = current;
            }

            return nearestPosition;
        }

        public BoardPosition GetMiddlePosition(BoardModel model) => new(model.Width / 2, model.Height / 2);

        public IEnumerable<BoardPosition> GetAdjacentOfType(BoardModel model, BoardPosition position, ItemType type)
        {
            BoardPosition[] adjacentPositions =
            {
                position + BoardPosition.Up, position + BoardPosition.Right, position + BoardPosition.Down,
                position + BoardPosition.Left
            };

            foreach (BoardPosition adjacent in adjacentPositions)
            {
                if (model.IsPositionValid(adjacent) && model.GetItem(adjacent) == type)
                    yield return adjacent;
            }
        }
    }
}
