using Model;
using UnityEngine;

namespace View
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sprite;
        public ItemType Type { get; private set; }

        public void Initialize(ItemType type)
        {
            Type = type;
        }

        public void SetOrder(int order)
        {
            sprite.sortingOrder = order;
        }
    }
}
