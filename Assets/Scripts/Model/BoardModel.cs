using UnityEngine.Assertions;

namespace Model
{
    public class BoardModel
    {
        private readonly FieldType[,] fields;

        public int Width => fields.GetLength(0);
        public int Height => fields.GetLength(1);

        public BoardModel(int width, int height)
        {
            fields = new FieldType[width, height];
        }

        public void SetField(int x, int y, FieldType type)
        {
            Assert.IsTrue(x >= 0 && x < Width);
            Assert.IsTrue(y >= 0 && y < Height);

            fields[x, y] = type;
        }

        public FieldType GetField(int x, int y)
        {
            Assert.IsTrue(x >= 0 && x < Width);
            Assert.IsTrue(y >= 0 && y < Height);

            return fields[x, y];
        }
    }
}
