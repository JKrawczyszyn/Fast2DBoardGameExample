using UnityEngine;

namespace View.Config
{
    [CreateAssetMenu(fileName = "ViewConfig", menuName = "ScriptableObjects/ViewConfig")]
    public class ViewConfig : ScriptableObject
    {
        public int targetFrameRate;
        public float spawnAnimationTime;
        public FieldsConfig fields;
        public ItemsConfig items;
    }
}
