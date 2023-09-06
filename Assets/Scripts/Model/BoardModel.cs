using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using Utilities;

namespace Model
{
    public class BoardModel
    {
        private readonly FieldType[,] fields;
        private readonly ItemType[,] items;

        private BoardPosition? spawnerPositionCache;

        public int Width { get; }
        public int Height { get; }

        public BoardModel(int width, int height)
        {
            Width = width;
            Height = height;

            fields = new FieldType[width, height];
            items = new ItemType[width, height];
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
            Assert.IsTrue(IsPositionOpen(position, ItemType.None, ItemType.Spawner));

            RemoveItems(ItemType.Spawner);
            SetItem(position, ItemType.Spawner);
        }

        public BoardPosition GetSpawner()
        {
            if (!spawnerPositionCache.HasValue
                || items[spawnerPositionCache.Value.X, spawnerPositionCache.Value.Y] != ItemType.Spawner)
                spawnerPositionCache = GetItemPositions(ItemType.Spawner).FirstOrDefault();

            return spawnerPositionCache ?? default;
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

        public bool IsPositionOpen(in BoardPosition position, params ItemType[] types)
        {
            Assert.IsTrue(IsPositionValid(position));

            if (types.Length == 0)
                types = new[] { ItemType.None };

            return GetField(position) == FieldType.Open && types.Contains(GetItem(position));
        }

        private void RemoveItems(ItemType type)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (items[x, y] == type)
                        items[x, y] = ItemType.None;
                }
            }
        }

        public void SetItem(in BoardPosition position, ItemType type)
        {
            Assert.IsTrue(IsPositionValid(position));
            Assert.IsTrue(IsPositionOpen(position));

            items[position.X, position.Y] = type;
        }

        private ItemType GetItem(in BoardPosition position)
        {
            Assert.IsTrue(IsPositionValid(position));

            return items[position.X, position.Y];
        }

        private IEnumerable<BoardPosition> GetItemPositions(ItemType type)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (items[x, y] == type)
                        yield return new BoardPosition(x, y);
                }
            }
        }

        public bool IsPositionValid(BoardPosition position) =>
            position.X.Between(0, Width - 1) && position.Y.Between(0, Height - 1);
    }
}
