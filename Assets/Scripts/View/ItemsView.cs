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

        private CancellationTokenSource cts = new();

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
        }

        private void CreateSpawner(BoardPosition boardPosition)
        {
            spawner = Instantiate(viewConfig.items.GetPrefab(ItemType.Spawner), transform) as Spawner;

            Assert.IsNotNull(spawner);

            spawner.transform.localPosition = coordConverter.BoardToWorld(boardPosition);
            spawner.Initialize(coordConverter);
            spawner.OnDragEnded += DragEnded;
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
                viewConfig.spawnAnimationTimeMilliseconds, cts.Token).Forget();

            items.Add(item);
        }

        private void OnDestroy()
        {
            spawner.OnDragEnded -= DragEnded;
            controller.OnSpawnerMoved -= SpawnerMoved;
            controller.OnItemSpawned -= ItemSpawned;
        }
    }
}
