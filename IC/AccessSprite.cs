using ItemChanger;
using KorzUtils.Helper;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace AccessRandomizer.IC
{
    [Serializable]
    public class AccessSprite : ISprite
    {
        #region Constructors

        public AccessSprite(string key)
        {
            if (!string.IsNullOrEmpty(key))
                Key = key;
        }

        #endregion

        #region Properties

        public string Key { get; set; }

        [JsonIgnore]
        public Sprite Value => SpriteHelper.CreateSprite<AccessRandomizer>("Sprites." + Key.Replace("/", ".").Replace("\\", "."));

        #endregion

        public ISprite Clone() => new AccessSprite(Key);
    }
}