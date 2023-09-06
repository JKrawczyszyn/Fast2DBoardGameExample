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

        public int Width => fields.GetLength(0);
        public int Height => fields.GetLength(1);

        public BoardModel(int width, int height)
        {
            fields = new FieldType[width, height];
            items = new ItemType[width, height];
        }

        public void SetField(in BoardPosition position, FieldType type)
        {
            Assert.IsTrue(position.X.Between(0, Width - 1));
            Assert.IsTrue(position.Y.Between(0, Height - 1));

            fields[position.X, position.Y] = type;
        }

        public FieldType GetField(in BoardPosition position)
        {
            Assert.IsTrue(position.X.Between(0, Width - 1));
            Assert.IsTrue(position.Y.Between(0, Height - 1));

            return fields[position.X, position.Y];
        }

        public void SetSpawner(in BoardPosition position)
        {
            Assert.IsTrue(IsPositionOpenAndEmpty(position));

            RemoveItems(ItemType.Spawner);
            SetItem(position, ItemType.Spawner);
        }

        public BoardPosition GetSpawner() => GetItemPositions(ItemType.Spawner).FirstOrDefault();

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

        public bool IsPositionOpenAndEmpty(in BoardPosition position)
        {
            Assert.IsTrue(position.X.Between(0, Width - 1));
            Assert.IsTrue(position.Y.Between(0, Height - 1));

            return GetField(position) == FieldType.Open && GetItem(position) == ItemType.None;
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

        private void SetItem(in BoardPosition position, ItemType type)
        {
            Assert.IsTrue(position.X.Between(0, Width - 1));
            Assert.IsTrue(position.Y.Between(0, Height - 1));

            items[position.X, position.Y] = type;
        }

        private ItemType GetItem(in BoardPosition position)
        {
            Assert.IsTrue(position.X.Between(0, Width - 1));
            Assert.IsTrue(position.Y.Between(0, Height - 1));

            return items[position.X, position.Y];
        }

        public IEnumerable<BoardPosition> GetItemPositions(ItemType type)
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
    }
}
