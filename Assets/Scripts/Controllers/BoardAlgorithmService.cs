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

                    if (GetDistanceSqr(position, BoardPosition.Zero) > maxDistanceSqr)
                        continue;

                    nearestPositions.Add(position);
                }
            }

            nearestPositions.Sort((position1, position2) =>
            {
                int distance1 = GetDistanceSqr(position1, BoardPosition.Zero);
                int distance2 = GetDistanceSqr(position2, BoardPosition.Zero);

                return distance1.CompareTo(distance2);
            });

            return nearestPositions.ToArray();
        }

        private int[] GenerateDistanceBreakupIndexes()
        {
            List<int> indexes = new();

            var currentDistance = 1;

            for (var i = 0; i < nearestOffsetsLookup.Length; i++)
            {
                BoardPosition position = nearestOffsetsLookup[i];

                if (GetDistanceSqr(position, BoardPosition.Zero) <= currentDistance * currentDistance)
                    continue;

                indexes.Add(i);

                currentDistance++;
            }

            indexes.Add(nearestOffsetsLookup.Length);

            return indexes.ToArray();
        }

        private int GetDistanceSqr(BoardPosition position1, BoardPosition position2)
        {
            int x = position1.X - position2.X;
            int y = position1.Y - position2.Y;

            return (x * x) + (y * y);
        }

        private float GetRealDistanceSqr(BoardPosition position1, Vector2 position2)
        {
            float x = position1.X - position2.x;
            float y = position1.Y - position2.y;

            return (x * x) + (y * y);
        }

        public BoardPosition GetClosestOpen(BoardModel model, BoardPosition position, ItemType[] types)
        {
            while (true)
            {
                foreach (BoardPosition offset in nearestOffsetsLookup)
                {
                    BoardPosition current = position + offset;

                    if (!current.X.Between(0, model.Width - 1) || !current.Y.Between(0, model.Height - 1))
                        continue;

                    if (model.GetField(current) != FieldType.Open || !model.IsItemOneOfType(current, types))
                        continue;

                    return current;
                }

                if (maxSearchDistance >= Mathf.Max(model.Width, model.Height))
                    return default;

                maxSearchDistance = Mathf.Min(maxSearchDistance + 2, model.Width, model.Height);

                GenerateLookups();
            }
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

                float distance = GetRealDistanceSqr(current, floatPosition);

                if (distance >= nearestDistance)
                    continue;

                nearestDistance = distance;
                nearestPosition = current;
            }

            return nearestPosition;
        }

        public HashSet<BoardPosition> GetAdjacentPositions(BoardModel model)
        {
            HashSet<BoardPosition> result = new();

            for (var x = 0; x < model.Width; x++)
            {
                for (var y = 0; y < model.Height; y++)
                {
                    BoardPosition position = new(x, y);

                    if (model.GetField(position) == FieldType.Blocked)
                        continue;

                    ItemType type = model.GetItem(position);

                    if (type == ItemType.None)
                        continue;

                    bool adjacent = x < model.Width - 1 && Find(model, x + 1, y, type, ref result);
                    adjacent = adjacent || (y < model.Height - 1 && Find(model, x, y + 1, type, ref result));

                    if (adjacent)
                        result.Add(position);
                }
            }

            return result;
        }

        private bool Find(BoardModel model, int x, int y, ItemType type, ref HashSet<BoardPosition> result)
        {
            BoardPosition position = new(x, y);

            if (model.GetItem(position) != type)
                return false;

            result.Add(position);

            return true;
        }

        public BoardPosition GetMiddlePosition(BoardModel model) => new(model.Width / 2, model.Height / 2);
    }
}
