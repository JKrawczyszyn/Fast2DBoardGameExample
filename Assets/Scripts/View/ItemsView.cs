using Controller;
using Model;
using UnityEngine;
using Utilities;

namespace View
{
    public class ItemsView : MonoBehaviour
    {
        private AssetsRepository assetsRepository;
        private BoardController controller;
        private CoordConverter coordConverter;

        private Spawner spawner;

        private void Awake()
        {
            Inject();
        }

        private void Inject()
        {
            assetsRepository = DiManager.Instance.Resolve<AssetsRepository>();
            controller = DiManager.Instance.Resolve<BoardController>();
            coordConverter = DiManager.Instance.Resolve<CoordConverter>();
        }

        private void Start()
        {
            BoardPosition spawnerPosition = controller.GetSpawner();
            CreateSpawner(spawnerPosition);

            controller.OnSpawnerMoved += SpawnerMoved;
        }

        private void CreateSpawner(BoardPosition boardPosition)
        {
            spawner = Instantiate(assetsRepository.spawnerConfig.prefab, transform);
            spawner.transform.localPosition = coordConverter.BoardToWorld(boardPosition);
            spawner.Initialize(coordConverter);
            spawner.OnDragEnded += DragEnded;
        }

        private void DragEnded(Vector2 position)
        {
            BoardPosition boardPosition
                = BoardHelpers.GetClosestOpenAndEmpty(controller.Model, coordConverter, position);

            controller.SpawnerMove(boardPosition);
        }

        private void SpawnerMoved(BoardPosition position)
        {
            spawner.transform.localPosition = coordConverter.BoardToWorld(position);
        }

        private void OnDestroy()
        {
            spawner.OnDragEnded -= DragEnded;
            controller.OnSpawnerMoved -= SpawnerMoved;
        }
    }
}
