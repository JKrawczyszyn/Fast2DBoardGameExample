using System;
using Model;
using UnityEngine.Assertions;
using Utilities;

namespace Controller
{
    public class BoardController
    {
        public event Action<BoardPosition> OnSpawnerMoved;

        public BoardModel Model { get; }

        public int Width => Model.Width;
        public int Height => Model.Height;

        public BoardController(BoardModel model)
        {
            Model = model;

            BoardPosition middlePosition = BoardHelpers.GetMiddle(model.Width, model.Height);
            BoardPosition validMiddlePosition = BoardHelpers.GetClosestOpenAndEmpty(model, middlePosition);
            model.SetSpawner(validMiddlePosition);
        }

        public FieldType GetField(BoardPosition position) => Model.GetField(position);

        public BoardPosition GetSpawner()
        {
            BoardPosition position = Model.GetSpawner();

            Assert.IsFalse(position == default);

            return position;
        }

        public void SpawnerMove(BoardPosition position)
        {
            Model.SetSpawner(position);

            BoardPosition boardPosition = Model.GetSpawner();

            Assert.IsFalse(boardPosition == default);

            OnSpawnerMoved?.Invoke(boardPosition);
        }
    }
}
