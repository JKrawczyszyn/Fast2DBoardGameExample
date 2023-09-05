using System;
using UnityEngine;

namespace View
{
    [CreateAssetMenu(fileName = "AssetsRepository", menuName = "ScriptableObjects/AssetsRepository")]
    public class AssetsRepository : ScriptableObject
    {
        public FieldsConfig fieldsConfig;
    }

    [Serializable]
    public class FieldsConfig
    {
        public Field prefab;
        public Color color1, color2, colorBlocked;
    }
}
