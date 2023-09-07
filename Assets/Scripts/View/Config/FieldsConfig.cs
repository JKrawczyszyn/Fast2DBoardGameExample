using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine.Assertions;

namespace View.Config
{
    [Serializable]
    public class FieldsConfig
    {
        public FieldConfig[] configs;

        private Dictionary<int, Field> cache;

        public Field GetPrefab(FieldType type)
        {
            cache ??= configs.ToDictionary(c => (int)c.type, c => c.prefab);

            bool success = cache.TryGetValue((int)type, out Field field);

            Assert.IsTrue(success, $"Prefab for field type '{type}' not found.");

            return field;
        }

        [Serializable]
        public class FieldConfig
        {
            public FieldType type;
            public Field prefab;
        }
    }
}
