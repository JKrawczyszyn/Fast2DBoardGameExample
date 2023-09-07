using Controller;
using Model;
using UnityEngine;
using Utilities;
using View.Config;

namespace View
{
    public class FieldsView : MonoBehaviour
    {
        private ViewConfig viewConfig;
        private BoardController controller;
        private CoordConverter coordConverter;

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
            Field field = Instantiate(viewConfig.fields.GetPrefab(type), transform);
            field.transform.localPosition = coordConverter.BoardToWorld(boardPosition);

            return field;
        }
    }
}
