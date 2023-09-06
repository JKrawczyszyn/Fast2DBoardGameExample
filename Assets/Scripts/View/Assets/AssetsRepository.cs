using UnityEngine;

namespace View.Assets
{
    [CreateAssetMenu(fileName = "AssetsRepository", menuName = "ScriptableObjects/AssetsRepository")]
    public class AssetsRepository : ScriptableObject
    {
        public FieldsConfig fieldsConfig;
        public ItemsConfig itemsConfig;
    }
}
