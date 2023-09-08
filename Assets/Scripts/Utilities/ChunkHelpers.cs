using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class ChunkHelpers
    {
        public static IEnumerable<(Vector2Int offset, Vector2Int size)> CalculateChunks(int width, int height,
            Vector2Int maxSize)
        {
            for (var x = 0; x < width; x += maxSize.x)
            {
                for (var y = 0; y < height; y += maxSize.y)
                {
                    Vector2Int offset = new(x, y);
                    Vector2Int size = new(Mathf.Min(maxSize.x, width - x), Mathf.Min(maxSize.y, height - y));

                    yield return (offset, size);
                }
            }
        }

        public static Vector3 RelativeCenteredPosition(int width, int height,
            (Vector2Int offset, Vector2Int size) chunk)
        {
            float x = chunk.offset.x + ((chunk.size.x - width) / 2f);
            float y = chunk.offset.y + ((chunk.size.y - height) / 2f);

            return new Vector3( x, y);
        }
    }
}
