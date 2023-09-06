using System;
using Model;

namespace Controller
{
    public class BoardController
    {
        public event Action<BoardPosition> OnSpawnerMoved;

        private readonly BoardModel model;

        public int Width => model.Width;
        public int Height => model.Height;

        public BoardController(BoardModel model)
        {
            this.model = model;

            BoardPosition middlePosition = BoardHelpers.GetMiddle(model.Width, model.Height);
            BoardPosition validMiddlePosition = BoardHelpers.GetClosestOpen(model, middlePosition);
            model.SetSpawner(validMiddlePosition);
        }

        public FieldType GetField(BoardPosition position) => model.GetField(position);

        public BoardPosition GetSpawner() => model.GetSpawner();

        public void SpawnerMove(BoardPosition position)
        {
            BoardPosition validPosition = BoardHelpers.GetClosestOpen(model, position);
            model.SetSpawner(validPosition);

            OnSpawnerMoved?.Invoke(model.GetSpawner());
        }
    }
}
