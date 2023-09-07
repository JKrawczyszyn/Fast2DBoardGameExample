using Controller;
using Model;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;
using View.Config;

namespace View
{
    public class SpawnerView : MonoBehaviour
    {
        private ViewConfig viewConfig;
        private BoardController controller;
        private CoordConverter coordConverter;

        private Spawner spawner;

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
        }

        private void SpawnerMoved(BoardPosition position)
        {
            spawner.transform.localPosition = coordConverter.BoardToWorld(position);
        }

        private void CreateSpawner(BoardPosition boardPosition)
        {
            Item item = CreateItem(boardPosition, ItemType.Spawner);

            spawner = item as Spawner;

            Assert.IsNotNull(spawner);

            spawner.Initialize(coordConverter);
            spawner.OnDragEnded += DragEnded;
        }

        private Item CreateItem(BoardPosition position, ItemType type)
        {
            Item item = Instantiate(viewConfig.items.GetPrefab(type), transform);

            Assert.IsNotNull(item);

            item.transform.localPosition = coordConverter.BoardToWorld(position);

            return item;
        }

        private void DragEnded(Vector2 position)
        {
            BoardPosition boardPosition = BoardHelpers.GetClosestOpen(controller.Model, coordConverter, position,
                ItemType.None, ItemType.Spawner);

            controller.SpawnerMove(boardPosition);
        }

        private void OnDestroy()
        {
            spawner.OnDragEnded -= DragEnded;
            controller.OnSpawnerMoved -= SpawnerMoved;
        }
    }
}
