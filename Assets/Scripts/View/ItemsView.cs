using System.Collections.Generic;
using System.Threading;
using Controller;
using Model;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;
using View.Assets;

namespace View
{
    public class ItemsView : MonoBehaviour
    {
        private ViewConfig viewConfig;
        private BoardController controller;
        private CoordConverter coordConverter;

        private readonly List<Item> items = new();
        private Spawner spawner;

        private CancellationTokenSource animationsCts = new();

        private void Awake()
        {
            Inject();
        }

        private void Inject()
        {
            viewConfig = DiManager.Instance.Resolve<ViewConfig>();
            controller = DiManager.Instance.Resolve<BoardController>();
            coordConverter = DiManager.Instance.Resolve<CoordConverter>();
        }

        private void Start()
        {
            BoardPosition spawnerPosition = controller.GetSpawner();
            CreateSpawner(spawnerPosition);

            controller.OnSpawnerMoved += SpawnerMoved;
            controller.OnItemSpawned += ItemSpawned;
            controller.OnRefreshItems += RefreshItems;
        }

        private void CreateSpawner(BoardPosition boardPosition)
        {
            Item item = CreateItem(boardPosition, ItemType.Spawner);

            SetSpawner(item);
        }

        private void DragEnded(Vector2 position)
        {
            BoardPosition boardPosition = BoardHelpers.GetClosestOpen(controller.Model, coordConverter, position,
                ItemType.None, ItemType.Spawner);

            controller.SpawnerMove(boardPosition);
        }

        private void SpawnerMoved(BoardPosition position)
        {
            spawner.transform.localPosition = coordConverter.BoardToWorld(position);
        }

        private void ItemSpawned(BoardPosition start, BoardPosition end, ItemType type)
        {
            Item item = Instantiate(viewConfig.items.GetPrefab(type), transform);

            Assert.IsNotNull(item);

            Vector3 worldStartPosition = coordConverter.BoardToWorld(start);
            Vector3 worldEndPosition = coordConverter.BoardToWorld(end);

            item.transform.AnimateMoveLocal(worldStartPosition, worldEndPosition,
                viewConfig.spawnAnimationTimeMilliseconds, animationsCts.Token).Forget();

            items.Add(item);
        }

        private void RefreshItems(IEnumerable<(BoardPosition position, ItemType type)> currentItems)
        {
            CancelAnimations();
            RemoveItems();
            CreateItems(currentItems);
        }

        private void CancelAnimations()
        {
            animationsCts.Cancel();
            animationsCts.Dispose();
            animationsCts = new CancellationTokenSource();
        }

        private void RemoveItems()
        {
            foreach (Item item in items)
                Destroy(item.gameObject);

            items.Clear();

            UnsetSpawner();
        }

        private void CreateItems(IEnumerable<(BoardPosition position, ItemType type)> currentItems)
        {
            foreach ((BoardPosition position, ItemType type) in currentItems)
            {
                Item item = CreateItem(position, type);

                if (type == ItemType.Spawner)
                    SetSpawner(item);
            }
        }

        private Item CreateItem(BoardPosition position, ItemType type)
        {
            Item item = Instantiate(viewConfig.items.GetPrefab(type), transform);

            Assert.IsNotNull(item);

            item.transform.localPosition = coordConverter.BoardToWorld(position);

            items.Add(item);

            return item;
        }

        private void SetSpawner(Item item)
        {
            spawner = item as Spawner;

            Assert.IsNotNull(spawner);

            spawner.Initialize(coordConverter);
            spawner.OnDragEnded += DragEnded;
        }

        private void UnsetSpawner()
        {
            spawner.OnDragEnded -= DragEnded;
            spawner = null;
        }

        private void OnDestroy()
        {
            CancelAnimations();

            spawner.OnDragEnded -= DragEnded;
            controller.OnSpawnerMoved -= SpawnerMoved;
            controller.OnItemSpawned -= ItemSpawned;
            controller.OnRefreshItems -= RefreshItems;
        }
    }
}
