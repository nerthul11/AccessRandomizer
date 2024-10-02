using ItemChanger;
using ItemChanger.Internal;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace AccessRandomizer.IC
{
    [Serializable]
    public class AccessSprite : ISprite
    {
        private static SpriteManager EmbeddedSpriteManager = new(typeof(AccessSprite).Assembly, "AccessRandomizer.Resources.Sprites.");
        public string Key { get; set; }
        public AccessSprite(string key)
        {
            if (!string.IsNullOrEmpty(key))
                Key = key;
        }
        [JsonIgnore]
        public Sprite Value => EmbeddedSpriteManager.GetSprite(Key);
        public ISprite Clone() => (ISprite)MemberwiseClone();
    }
}