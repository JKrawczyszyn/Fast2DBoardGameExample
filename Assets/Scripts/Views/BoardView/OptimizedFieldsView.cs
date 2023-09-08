using System.Collections.Generic;
using Controllers;
using Models;
using UnityEngine;
using Utilities;
using Views.Config;

namespace Views
{
    public class OptimizedFieldsView : MonoBehaviour
    {
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
            IEnumerable<(Vector2Int offset, Vector2Int size)> chunks
                = ChunkHelpers.CalculateChunks(controller.Width, controller.Height, viewConfig.fields.maxTextureSize);

            foreach ((Vector2Int offset, Vector2Int size) chunk in chunks)
            {
                MeshRenderer meshRenderer = Instantiate(viewConfig.fields.chunkPrefab, Vector3.zero,
                    Quaternion.identity, transform);

                meshRenderer.material.mainTexture = CreateTexture(chunk);
                meshRenderer.transform.localScale = new Vector2(chunk.size.x, chunk.size.y);

                meshRenderer.transform.localPosition
                    = ChunkHelpers.RelativeCenteredPosition(controller.Width, controller.Height, chunk);

                meshRenderer.gameObject.SetActive(true);
            }
        }

        private Texture CreateTexture((Vector2Int offset, Vector2Int size) chunk)
        {
            int length = chunk.size.x * chunk.size.y;

            var colors = new Color[length];

            for (var i = 0; i < length; i++)
            {
                int x = i % chunk.size.x;
                int y = i / chunk.size.x;

                BoardPosition boardPosition = new(x + chunk.offset.x, y + chunk.offset.y);
                FieldType type = controller.GetField(boardPosition);

                bool alternate = (x + y) % 2 == 1;
                colors[i] = viewConfig.fields.GetColor(type, alternate);
            }

            Texture2D texture = new(chunk.size.x, chunk.size.y);
            texture.SetPixels(colors);
            texture.Apply();
            texture.filterMode = FilterMode.Point;

            return texture;
        }
    }
}
