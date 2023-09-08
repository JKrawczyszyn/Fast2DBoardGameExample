using Controllers;
using Models;
using UnityEngine;
using Utilities;
using Views.Config;

namespace Views
{
    public class FieldsView : MonoBehaviour
    {
        private ViewConfig viewConfig;
        private BoardController controller;
        private CoordConverter coordConverter;

        private ItemsFactory factory;

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

                    Field field = CreateField(boardPosition);

                    bool alternate = (x + y) % 2 == 1;
                    Color color = viewConfig.fields.GetColor(type, alternate);

                    field.Initialize(color);
                }
            }
        }

        private Field CreateField(BoardPosition boardPosition)
        {
            Field field = Instantiate(viewConfig.fields.prefab, transform);
            field.transform.localPosition = coordConverter.BoardToWorld(boardPosition);

            return field;
        }
    }
}
