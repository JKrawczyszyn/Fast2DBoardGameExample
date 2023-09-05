using System;
using UnityEngine;

namespace View
{
    [CreateAssetMenu(fileName = "AssetsRepository", menuName = "ScriptableObjects/AssetsRepository")]
    public class AssetsRepository : ScriptableObject
    {
        public FieldsConfig fieldsConfig;
        public SpawnerConfig spawnerConfig;
    }

    [Serializable]
    public class FieldsConfig
    {
        public Field prefab;
        public Color color1, color2, colorBlocked;
    }

    [Serializable]
    public class SpawnerConfig
    {
        public Spawner prefab;
    }
}
