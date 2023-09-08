using UnityEngine;

namespace Views
{
    public class Field : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public void Initialize(Color color)
        {
            spriteRenderer.color = color;
        }
    }
}
