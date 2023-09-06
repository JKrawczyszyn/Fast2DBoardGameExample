using UnityEngine;

namespace View.Assets
{
    [CreateAssetMenu(fileName = "ViewConfig", menuName = "ScriptableObjects/ViewConfig")]
    public class ViewConfig : ScriptableObject
    {
        public int targetFrameRate;
        public int spawnAnimationTimeMilliseconds;
        public FieldsConfig fields;
        public ItemsConfig items;
    }
}
