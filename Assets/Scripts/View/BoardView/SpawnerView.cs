using Controller;
using Model;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;

namespace View
{
    public class SpawnerView : MonoBehaviour
    {
        private BoardController controller;
        private CoordConverter coordConverter;
        private ItemsFactory itemsFactory;

        private Spawner spawner;

        private void Awake()
        {
            Inject();
        }

        private void Inject()
        {
            controller = DiManager.Instance.Resolve<BoardController>();
            coordConverter = DiManager.Instance.Resolve<CoordConverter>();
            itemsFactory = DiManager.Instance.Resolve<ItemsFactory>();
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
            Item item = itemsFactory.Create(ItemType.Spawner);

            spawner = item as Spawner;

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

        private void OnDestroy()
        {
            spawner.OnDragEnded -= DragEnded;
            controller.OnSpawnerMoved -= SpawnerMoved;
        }
    }
}
