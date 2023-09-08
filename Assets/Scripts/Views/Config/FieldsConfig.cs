using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace Views.Config
{
    [Serializable]
    public class FieldsConfig
    {
        public bool optimized;
        public Field prefab;
        public MeshRenderer chunkPrefab;
        public Vector2Int maxTextureSize;
        public FieldConfig[] configs;

        [NonSerialized]
        private FieldConfig[] cache;

        [NonSerialized]
        private FieldConfig[] cacheAlternate;

        public Color GetColor(FieldType type, bool alternate = false)
        {
            CreateCacheIfShould();

            FieldConfig fieldConfig = alternate ? cacheAlternate[(int)type] : cache[(int)type];

            return fieldConfig.color;
        }

        private void CreateCacheIfShould()
        {
            if (cache != null && cacheAlternate != null)
                return;

            // Assume we are using standard enum without gaps
            int length = Enum.GetValues(typeof(FieldType)).Length;
            cache = new FieldConfig[length];
            cacheAlternate = new FieldConfig[length];

            foreach (FieldConfig config in configs)
            {
                if (config.alternate)
                    cacheAlternate[(int)config.type] = config;
                else
                {
                    cache[(int)config.type] = config;
                    cacheAlternate[(int)config.type] ??= config;
                }
            }

            Assert.IsTrue(cache.All(c => c != null));
            Assert.IsTrue(cacheAlternate.All(c => c != null));
        }

        [Serializable]
        public class FieldConfig
        {
            public FieldType type;
            public bool alternate;
            public Color color;
        }
    }
}
