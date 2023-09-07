using Model;
using UnityEngine.Assertions;
using Utilities;

namespace View
{
    public class ItemsFactory
    {
        private ItemsPooler pooler;

        public ItemsFactory()
        {
            Inject();
        }

        private void Inject()
        {
            pooler = DiManager.Instance.Resolve<ItemsPooler>();
        }

        public Item Create(ItemType type)
        {
            Item item = pooler.Get(type);

            Assert.IsNotNull(item);

            item.Initialize(type);

            return item;
        }

        public void Destroy(Item item)
        {
            pooler.Release(item);
        }
    }
}
