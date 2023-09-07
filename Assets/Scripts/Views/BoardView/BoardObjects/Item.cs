using Models;
using UnityEngine;

namespace Views
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public ItemType Type { get; private set; }

        public void Initialize(ItemType type)
        {
            Type = type;
        }

        public void SetOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }
    }
}
