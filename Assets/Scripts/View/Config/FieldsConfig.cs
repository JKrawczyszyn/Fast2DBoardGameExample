using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace View.Config
{
    [Serializable]
    public class FieldsConfig
    {
        public bool optimized;
        public Field prefab;
        public Vector2Int tileSize;
        public FieldConfig[] configs;

        private Dictionary<(int, bool), Sprite> cache;

        public Sprite GetSprite(FieldType type, bool alternate = false)
        {
            cache ??= configs.ToDictionary(c => ((int)c.type, c.alternate), c => c.sprite);

            bool success = cache.TryGetValue(((int)type, alternate), out Sprite field);
            if (!success)
                success = cache.TryGetValue(((int)type, false), out field);

            Assert.IsTrue(success, $"Sprite for field type '{type}' not found.");

            return field;
        }

        [Serializable]
        public class FieldConfig
        {
            public FieldType type;
            public bool alternate;
            public Sprite sprite;
        }
    }
}
