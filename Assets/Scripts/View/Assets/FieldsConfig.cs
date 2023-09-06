using System;
using System.Linq;
using Model;
using UnityEngine.Assertions;

namespace View.Assets
{
    [Serializable]
    public class FieldsConfig
    {
        public FieldConfig[] configs;

        public Field GetPrefab(FieldType type)
        {
            Field field = configs.Where(c => c.type == type).Select(c => c.prefab).FirstOrDefault();

            Assert.IsNotNull(field, $"Prefab for '{type}' not found.");

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
