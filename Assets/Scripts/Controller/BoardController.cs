using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine.Assertions;
using Utilities;
using Random = UnityEngine.Random;

namespace Controller
{
    public class BoardController
    {
        public event Action<BoardPosition> OnSpawnerMoved;
        public event Action<BoardPosition, BoardPosition, ItemType> OnItemSpawned;
        public event Action<IEnumerable<(BoardPosition position, ItemType type)>> OnRefreshItems;

        private readonly SpawnController spawnController;

        public BoardModel Model { get; }

        public int Width => Model.Width;
        public int Height => Model.Height;

        public BoardController(BoardModel model)
        {
            Model = model;

            spawnController = DiManager.Instance.Resolve<SpawnController>();

            BoardPosition middlePosition = BoardHelpers.GetMiddle(model.Width, model.Height);
            BoardPosition validMiddlePosition = BoardHelpers.GetClosestOpen(model, middlePosition);
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
            if (position != default)
                Model.SetSpawner(position);

            BoardPosition boardPosition = Model.GetSpawner();

            Assert.IsFalse(boardPosition == default);

            OnSpawnerMoved?.Invoke(boardPosition);
        }

        public void ResetSpawner()
        {
            spawnController.Reset();
        }

        public void SpawnItem()
        {
            BoardPosition startPosition = Model.GetSpawner();

            BoardPosition position = spawnController.FindNextFreeSpawnPosition(Model, startPosition);
            if (position == default)
                return;

            ItemType itemType = GetRandomItemType();

            Model.SetItem(position, itemType);

            OnItemSpawned?.Invoke(startPosition, position, itemType);
        }

        private ItemType GetRandomItemType()
        {
            ItemType[] types = { ItemType.Item1, ItemType.Item2, ItemType.Item3 };

            return types[Random.Range(0, types.Length)];
        }

        public void ClearAdjacent()
        {
            var toRemove = BoardHelpers.GetAdjacentPositions(Model);
            foreach (BoardPosition position in toRemove)
                Model.RemoveItem(position);

            OnRefreshItems?.Invoke(Model.GetItems());
        }
    }
}
