using UnityEngine;
using UnityEngine.Assertions;

namespace View
{
    public class Field : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer[] sprites;

        public void Initialize(int x, int y)
        {
            Assert.IsTrue(sprites.Length.Between(1, 2));

            if (sprites.Length != 2)
                return;

            int index = (x + y) % 2;
            sprites[index].gameObject.SetActive(true);
            sprites[1 - index].gameObject.SetActive(false);
        }
    }
}
