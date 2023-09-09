using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class BoardController
    {
        public event Action<BoardPosition> OnSpawnerMoved;
        public event Action<BoardPosition, BoardPosition, ItemType> OnItemSpawned;
        public event Action<(BoardPosition position, ItemType type)[]> OnRefreshItems;

        private readonly ItemType[] itemTypes = { ItemType.Item1, ItemType.Item2, ItemType.Item3 };

        private SpawnController spawnController;
        private BoardAlgorithmService boardAlgorithmService;

        private readonly BoardModel model;

        public int Width => model.Width;
        public int Height => model.Height;

        public BoardController(BoardModel model)
        {
            this.model = model;

            Inject();

            BoardPosition middlePosition = boardAlgorithmService.GetMiddlePosition(model);

            BoardPosition validMiddlePosition
                = boardAlgorithmService.GetClosestOpen(model, middlePosition.ToVector2(), new[] { ItemType.None });

            model.SetSpawner(validMiddlePosition);
        }

        private void Inject()
        {
            spawnController = DiManager.Instance.Resolve<SpawnController>();
            boardAlgorithmService = DiManager.Instance.Resolve<BoardAlgorithmService>();
        }

        public FieldType GetField(BoardPosition position) => model.GetField(position);

        public ItemType GetItem(BoardPosition position) => model.GetItem(position);

        public bool GetClear(BoardPosition position) => model.GetClear(position);

        public BoardPosition GetSpawner()
        {
            BoardPosition position = model.GetSpawner();

            Assert.IsFalse(position == default);

            return position;
        }

        public void SpawnerMove(Vector2 position)
        {
            SetSpawner(position);

            BoardPosition boardPosition = model.GetSpawner();

            Assert.IsFalse(boardPosition == default);

            OnSpawnerMoved?.Invoke(boardPosition);
        }

        private void SetSpawner(Vector2 position)
        {
            BoardPosition boardPosition
                = boardAlgorithmService.GetClosestOpen(model, position, new[] { ItemType.None, ItemType.Spawner });

            Assert.IsFalse(boardPosition == default);

            model.SetSpawner(boardPosition);
        }

        public void ResetSpawner()
        {
            spawnController.Reset();
        }

        public void SpawnItem()
        {
            BoardPosition startPosition = model.GetSpawner();

            BoardPosition position = spawnController.FindNextFreeSpawnPosition(model, startPosition);

            if (position == default)
                return;

            ItemType itemType = GetRandomItemType();

            model.SetItem(position, itemType);

            SetClearStatuses(position, itemType);

            OnItemSpawned?.Invoke(startPosition, position, itemType);
        }

        private void SetClearStatuses(BoardPosition position, ItemType itemType)
        {
            IEnumerable<BoardPosition> adjacent = boardAlgorithmService.GetAdjacentOfType(model, position, itemType);

            BoardPosition[] adjacentArray = adjacent as BoardPosition[] ?? adjacent.ToArray();

            if (adjacentArray.Length == 0)
                return;

            model.SetClear(position);
            model.SetClear(adjacentArray);
        }

        private ItemType GetRandomItemType() => itemTypes[Random.Range(0, itemTypes.Length)];

        public void ClearAdjacent()
        {
            IEnumerable<BoardPosition> toRemove = model.GetClear();

            foreach (BoardPosition positionToRemove in toRemove.ToArray())
            {
                ItemType type = model.GetItem(positionToRemove);

                model.RemoveItem(positionToRemove);
                model.RemoveClear(positionToRemove);

                IEnumerable<BoardPosition> adjacent = boardAlgorithmService.GetAdjacentOfType(model, positionToRemove, type);
                foreach (BoardPosition adjacentPosition in adjacent)
                    RemoveClearIfNoAdjacent(adjacentPosition, type);
            }

            OnRefreshItems?.Invoke(model.GetItems(itemTypes).ToArray());
        }

        private void RemoveClearIfNoAdjacent(BoardPosition position, ItemType type)
        {
            IEnumerable<BoardPosition> adjacent = boardAlgorithmService.GetAdjacentOfType(model, position, type);
            if (!adjacent.Any())
                model.RemoveClear(position);
        }
    }
}
