using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine.Assertions;

namespace View.Config
{
    [Serializable]
    public class ItemsConfig
    {
        public ItemConfig[] configs;

        private Dictionary<int, Item> cache;

        public Item GetPrefab(ItemType type)
        {
            cache ??= configs.ToDictionary(c => (int)c.type, c => c.prefab);

            bool success = cache.TryGetValue((int)type, out Item field);

            Assert.IsTrue(success, $"Prefab for item type '{type}' not found.");

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
