using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using UnityEngine;

namespace AccessRandomizer.IC
{
    public class SlyKeyItem : AbstractItem
    {
        public override bool Redundant() => AccessModule.Instance.UnlockedSly;
        public override void GiveImmediate(GiveInfo info)
        {
            AccessModule.Instance.UnlockedSly = true;

            // If on same scene, open the door
            if (GameManager._instance.sceneName == SceneNames.Crossroads_04)
            {
                GameObject door = GameObject.Find("Mender Door(Clone)(Clone)");
                door.FindChild("Door Closed").SetActive(false);
                door.FindChild("Door Open").SetActive(true);
            }
            AccessModule.Instance.CompletedChallenges();
        }

        public SlyKeyItem()
        {
            name = "Sly_Key";
            UIDef = new MsgUIDef()
            {
                name = new BoxedString("Sly Key"),
                shopDesc = new BoxedString("Now where did I leave this one? -Cornifer"),
                sprite = new AccessSprite("SlyKey")
            };
            tags = [ItemTag(), CurseTag()];
        }
        private static Tag ItemTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("SlyKey");
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
        private InteropTag CurseTag()
        {
            InteropTag tag = new();
            tag.Properties["CanMimic"] = new BoxedBool(true);
            tag.Properties["MimicNames"] = new string[] {"Slay Key", "S1y Key", "Sly Kee"};
            tag.Message = "CurseData";
            return tag;
        }
    }
}