using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using Utilities;

namespace Models
{
    public class BoardModel
    {
        private readonly FieldType[,] fields;
        private readonly Dictionary<BoardPosition, ItemType> items;
        private readonly HashSet<BoardPosition> toClear;

        private BoardPosition? spawnerPositionCache;

        public int Width { get; }
        public int Height { get; }

        public BoardModel(int width, int height)
        {
            Width = width;
            Height = height;

            fields = new FieldType[width, height];
            items = new Dictionary<BoardPosition, ItemType>();
            toClear = new HashSet<BoardPosition>();
        }

        public void SetField(in BoardPosition position, FieldType type)
        {
            Assert.IsTrue(IsPositionValid(position));

            fields[position.X, position.Y] = type;
        }

        public FieldType GetField(in BoardPosition position)
        {
            Assert.IsTrue(IsPositionValid(position));

            return fields[position.X, position.Y];
        }

        public void SetSpawner(in BoardPosition position)
        {
            Assert.IsTrue(IsPositionValid(position));
            Assert.IsTrue(GetField(position) == FieldType.Open);
            Assert.IsTrue(IsItemOneOfType(position, new[] { ItemType.None, ItemType.Spawner }));

            RemoveItems(ItemType.Spawner);
            SetItem(position, ItemType.Spawner);

            spawnerPositionCache = position;
        }

        public BoardPosition GetSpawner()
        {
            spawnerPositionCache ??= GetItemPositions(ItemType.Spawner).FirstOrDefault();

            return spawnerPositionCache.Value;
        }

        public int CountFields(FieldType type)
        {
            var count = 0;

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (fields[x, y] == type)
                        count++;
                }
            }

            Assert.IsTrue(count.Between(0, Width * Height));

            return count;
        }

        public bool IsItemOneOfType(BoardPosition current, ItemType[] types)
        {
            Assert.IsTrue(IsPositionValid(current));
            Assert.IsTrue(types.Length > 0);

            foreach (ItemType type in types)
            {
                bool success = items.TryGetValue(current, out ItemType outType);

                if (!success && type == ItemType.None)
                    return true;

                if (success && outType == type)
                    return true;
            }

            return false;
        }

        private void RemoveItems(ItemType type)
        {
            foreach (BoardPosition position in GetItemPositions(type).ToArray())
                items.Remove(position);
        }

        public void RemoveItem(in BoardPosition position)
        {
            Assert.IsTrue(IsPositionValid(position));
            Assert.IsTrue(items.ContainsKey(position));

            items.Remove(position);
            toClear.Remove(position);
        }

        public void SetItem(in BoardPosition position, ItemType type)
        {
            Assert.IsTrue(IsPositionValid(position));
            Assert.IsTrue(GetField(position) == FieldType.Open);
            Assert.IsTrue(GetItem(position) == ItemType.None);
            Assert.IsFalse(items.ContainsKey(position));

            items.Add(position, type);
        }

        public ItemType GetItem(in BoardPosition position)
        {
            Assert.IsTrue(IsPositionValid(position));

            bool success = items.TryGetValue(position, out ItemType type);

            return !success ? ItemType.None : type;
        }

        public IEnumerable<(BoardPosition position, ItemType type)> GetItems(ItemType[] itemTypes) =>
            items.Where(pair => itemTypes.Contains(pair.Value)).Select(pair => (pair.Key, pair.Value));

        private IEnumerable<BoardPosition> GetItemPositions(ItemType type) =>
            items.Where(pair => pair.Value == type).Select(pair => pair.Key);

        public bool IsPositionValid(in BoardPosition position) =>
            position.X.Between(0, Width - 1) && position.Y.Between(0, Height - 1);

        public void SetClear(IEnumerable<BoardPosition> positions)
        {
            foreach (BoardPosition position in positions)
                SetClear(position);
        }

        public void SetClear(BoardPosition position)
        {
            Assert.IsTrue(IsPositionValid(position));

            toClear.Add(position);
        }

        public IEnumerable<BoardPosition> GetClear() => toClear;

        public bool GetClear(BoardPosition position)
        {
            Assert.IsTrue(IsPositionValid(position));

            return toClear.Contains(position);
        }

        public void RemoveClear(BoardPosition position)
        {
            Assert.IsTrue(IsPositionValid(position));

            toClear.Remove(position);
        }
    }
}
