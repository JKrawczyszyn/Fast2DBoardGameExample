using System;
using System.Linq;
using Model;
using UnityEngine.Assertions;

namespace View.Assets
{
    [Serializable]
    public class ItemsConfig
    {
        public ItemConfig[] configs;

        public Item GetPrefab(ItemType type)
        {
            Item field = configs.Where(c => c.type == type).Select(c => c.prefab).FirstOrDefault();

            Assert.IsNotNull(field, $"Prefab for '{type}' not found.");

            return field;
        }

        [Serializable]
        public class ItemConfig
        {
            public ItemType type;
            public Item prefab;
        }
    }
}
