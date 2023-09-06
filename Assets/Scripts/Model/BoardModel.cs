using UnityEngine.Assertions;

namespace Model
{
    public class BoardModel
    {
        private readonly FieldType[,] fields;

        public int Width => fields.GetLength(0);
        public int Height => fields.GetLength(1);

        private BoardPosition spawnerPosition;

        public BoardModel(int width, int height)
        {
            fields = new FieldType[width, height];
        }

        public void SetField(int x, int y, FieldType type)
        {
            Assert.IsTrue(x.Between(0, Width - 1));
            Assert.IsTrue(y.Between(0, Height - 1));

            fields[x, y] = type;
        }

        public FieldType GetField(BoardPosition position)
        {
            Assert.IsTrue(position.X.Between(0, Width - 1));
            Assert.IsTrue(position.Y.Between(0, Height - 1));

            return fields[position.X, position.Y];
        }

        public int CountFields(FieldType type)
        {
            var count = 0;

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (GetField(new BoardPosition(x, y)) == type)
                        count++;
                }
            }

            Assert.IsTrue(count.Between(0, Width * Height));

            return count;
        }

        public void SetSpawner(BoardPosition position)
        {
            Assert.IsTrue(position.X.Between(0, Width - 1));
            Assert.IsTrue(position.Y.Between(0, Height - 1));
            Assert.IsTrue(GetField(position) == FieldType.Open);

            spawnerPosition = position;
        }

        public BoardPosition GetSpawner()
        {
            Assert.IsFalse(spawnerPosition == default);

            return spawnerPosition;
        }
    }
}
