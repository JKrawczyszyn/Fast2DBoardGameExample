using Controller;
using Model;
using UnityEngine;

namespace View
{
    public class FieldsView : MonoBehaviour
    {
        private AssetsRepository assetsRepository;
        private BoardController controller;
        private CoordConverter coordConverter;

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
            CreateFields(controller.Width, controller.Height);
        }

        private void CreateFields(int width, int height)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    BoardPosition boardPosition = new(x, y);

                    FieldType type = controller.GetField(boardPosition);
                    Field field = CreateField(type, boardPosition);

                    field.Initialize(x, y);
                }
            }
        }

        private Field CreateField(FieldType type, BoardPosition boardPosition)
        {
            Field field = Instantiate(assetsRepository.fieldsConfig.GetPrefab(type), transform);
            field.transform.localPosition = coordConverter.BoardToWorld(boardPosition);

            return field;
        }
    }
}
