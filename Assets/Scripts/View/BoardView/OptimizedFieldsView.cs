using Controller;
using Model;
using UnityEngine;
using Utilities;
using View.Config;

namespace View
{
    public class OptimizedFieldsView : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;

        private ViewConfig viewConfig;
        private BoardController controller;

        private void Awake()
        {
            Inject();
        }

        private void Inject()
        {
            viewConfig = DiManager.Instance.Resolve<ViewConfig>();
            controller = DiManager.Instance.Resolve<BoardController>();
        }

        private void Start()
        {
            meshRenderer.transform.localScale = new Vector2(controller.Width, controller.Height);
            meshRenderer.material.mainTexture = CreateTexture(controller.Width, controller.Height);
            meshRenderer.gameObject.SetActive(true);
        }

        private Texture CreateTexture(int width, int height)
        {
            int tileWidth = viewConfig.fields.tileSize.x;
            int tileHeight = viewConfig.fields.tileSize.y;

            Texture2D texture = new(controller.Width * tileWidth, controller.Height * tileHeight);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    BoardPosition boardPosition = new(x, y);

                    FieldType type = controller.GetField(boardPosition);

                    bool alternate = (x + y) % 2 == 1;
                    Sprite sprite = viewConfig.fields.GetSprite(type, alternate);

                    Color[] pixels = sprite.texture.GetPixels(0, 0, tileWidth, tileHeight);

                    texture.SetPixels(x * tileWidth, y * tileHeight, tileWidth, tileHeight, pixels);
                }
            }

            texture.Apply();
            texture.filterMode = FilterMode.Point;

            return texture;
        }
    }
}
