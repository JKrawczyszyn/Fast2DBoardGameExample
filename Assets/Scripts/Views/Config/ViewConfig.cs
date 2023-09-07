using UnityEngine;

namespace Views.Config
{
    [CreateAssetMenu(fileName = "ViewConfig", menuName = "ScriptableObjects/ViewConfig")]
    public class ViewConfig : ScriptableObject
    {
        public int targetFrameRate;
        public bool showFps;
        public float spawnAnimationTime;
        public FieldsConfig fields;
        public ItemsConfig items;
    }
}
