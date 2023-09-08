using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.Pool;
using Utilities;
using Views.Config;

namespace Views
{
    public class ItemsPooler
    {
        private ViewConfig viewConfig;

        private readonly Transform parent;

        private readonly Dictionary<ItemType, IObjectPool<Item>> pools = new();

        public ItemsPooler(Transform parent)
        {
            this.parent = parent;

            Inject();
        }

        private void Inject()
        {
            viewConfig = DiManager.Instance.Resolve<ViewConfig>();
        }

        public Item Get(ItemType type)
        {
            if (!pools.TryGetValue(type, out IObjectPool<Item> pool))
            {
                pool = CreatePool(type);

                pools.Add(type, pool);
            }

            return pool.Get();
        }

        public void Release(Item item)
        {
            if (!pools.TryGetValue(item.Type, out IObjectPool<Item> pool))
            {
                Debug.LogError($"Pool for '{item.Type}' not found.");

                return;
            }

            pool.Release(item);
        }

        private IObjectPool<Item> CreatePool(ItemType type)
        {
            var pool = new ObjectPool<Item>(onCreate, onGet, onRelease, onDestroy);

            Item onCreate() => Object.Instantiate(viewConfig.items.GetPrefab(type), parent);

            void onGet(Item item)
            {
                item.gameObject.SetActive(true);
            }

            void onRelease(Item item)
            {
                item.gameObject.SetActive(false);
            }

            void onDestroy(Item item)
            {
                Object.Destroy(item.gameObject);
            }

            return pool;
        }
    }
}
