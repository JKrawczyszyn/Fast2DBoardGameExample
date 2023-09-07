using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Models
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = "ScriptableObjects/BoardConfig")]
    public class BoardConfig : ScriptableObject
    {
        [Range(0f, 1f)]
        public float blockedChance;

        private const string fileName = "BoardConfig";

        private Lazy<BoardConfigJson> config;

        public int Width => config.Value.Width;
        public int Height => config.Value.Height;

        public void Initialize()
        {
            config = new Lazy<BoardConfigJson>(LoadConfig);
        }

        private static BoardConfigJson LoadConfig()
        {
            TextAsset asset = Resources.Load<TextAsset>(fileName);
            string json = asset.text;

            BoardConfigJson configJson = JsonUtility.FromJson<BoardConfigJson>(json);

            Assert.IsTrue(configJson.Width >= 2, "Board width must be at least 2.");
            Assert.IsTrue(configJson.Height >= 2, "Board height must be at least 2.");

            return configJson;
        }

        private class BoardConfigJson
        {
            public int Width;
            public int Height;
        }
    }
}
