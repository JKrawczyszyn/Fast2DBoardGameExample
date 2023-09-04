using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;

    public void Initialize(AssetsRepository assetsRepository, FieldType type, int x, int y)
    {
        if (type == FieldType.Blocked)
        {
            sprite.color = assetsRepository.fieldsConfig.colorBlocked;

            return;
        }

        sprite.color = (x + y) % 2 == 1 ? assetsRepository.fieldsConfig.color2 : assetsRepository.fieldsConfig.color1;
    }
}
