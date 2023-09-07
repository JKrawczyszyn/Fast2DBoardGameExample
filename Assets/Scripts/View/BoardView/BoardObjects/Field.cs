using UnityEngine;

namespace View
{
    public class Field : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public void Initialize(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
