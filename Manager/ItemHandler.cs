using AccessRandomizer.IC;
using ItemChanger;
using ItemChanger.Tags;
using Newtonsoft.Json;
using RandomizerMod.IC;
using RandomizerMod.RC;
using RandomizerMod.Settings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AccessRandomizer.Manager {
    internal static class ItemHandler
    {
        internal static void Hook()
        {
            DefineObjects();
            RequestBuilder.OnUpdate.Subscribe(0f, AddObjects);
            RequestBuilder.OnUpdate.Subscribe(200f, AddGladeKey);
            RequestBuilder.OnUpdate.Subscribe(1200f, ReplaceKeys);
        }

        public static void DefineObjects()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            JsonSerializer jsonSerializer = new() {TypeNameHandling = TypeNameHandling.Auto};
            using Stream itemStream = assembly.GetManifestResourceStream("AccessRandomizer.Resources.Data.KeyItems.json");
            StreamReader itemReader = new(itemStream);
            List<KeyItem> itemList = jsonSerializer.Deserialize<List<KeyItem>>(new JsonTextReader(itemReader));

            foreach (KeyItem item in itemList)
                Finder.DefineCustomItem(item);
            
            Finder.DefineCustomItem(new RespectItem());
            Finder.DefineCustomLocation(new RespectLocation());

            Finder.DefineCustomItem(new ChainItem());
            Finder.DefineCustomLocation(new ChainLocation(1, 0.4f, -0.4f));
            Finder.DefineCustomLocation(new ChainLocation(2, 0.4f, -0.1f));
            Finder.DefineCustomLocation(new ChainLocation(3, 0.4f, 0.2f));
            Finder.DefineCustomLocation(new ChainLocation(4, 0.4f, 0.5f));

            Finder.DefineCustomItem(new MapperKeyItem());
            Finder.DefineCustomLocation(new MapperKeyLocation());

            Finder.DefineCustomItem(new GladeItem());
        }

        public static void AddObjects(RequestBuilder rb)
        {
            if (!AccessManager.Settings.Enabled)
                return;
            
            if (AccessManager.Settings.MantisRespect)
            {
                rb.AddItemByName("Mantis_Respect");
                if (rb.gs.DuplicateItemSettings.DuplicateUniqueKeys)
                    rb.AddItemByName($"{PlaceholderItem.Prefix}Mantis_Respect");
                rb.EditItemRequest("Mantis_Respect", info => 
                {
                    info.getItemDef = () => new()
                    {
                        MajorItem = false,
                        Name = "Mantis_Respect",
                        Pool = "Key",
                        PriceCap = 500
                    };
                });
                rb.AddLocationByName("Mantis_Respect");
                rb.EditLocationRequest("Mantis_Respect", info =>
                {
                    info.getLocationDef = () => new()
                    {
                        Name = "Mantis_Respect",
                        SceneName = SceneNames.Fungus2_15,
                        FlexibleCount = false,
                        AdditionalProgressionPenalty = false
                    };
                });
            }

            if (AccessManager.Settings.HollowKnightChains)
            {
                rb.AddItemByName("Hollow_Knight_Chain", 4);
                if (rb.gs.DuplicateItemSettings.Dreamer)
                    rb.AddItemByName($"{PlaceholderItem.Prefix}Hollow_Knight_Chain", 2);
                rb.EditItemRequest("Hollow_Knight_Chain", info => 
                {
                    info.getItemDef = () => new()
                    {
                        MajorItem = true,
                        Name = "Hollow_Knight_Chain",
                        Pool = "Dreamers",
                        PriceCap = 2000
                    };
                });
                foreach (int i in Enumerable.Range(1, 4))
                {
                    rb.AddLocationByName($"Hollow_Knight_Chain-{i}");
                    rb.EditLocationRequest($"Hollow_Knight_Chain-{i}", info =>
                    {
                        info.getLocationDef = () => new()
                        {
                            Name = $"Hollow_Knight_Chain-{i}",
                            SceneName = SceneNames.Room_Final_Boss_Core,
                            FlexibleCount = false,
                            AdditionalProgressionPenalty = false
                        };
                    });
                }
            }

            if (AccessManager.Settings.MapperKey)
            {
                rb.AddItemByName("Mapper_Key");
                rb.EditItemRequest("Mapper_Key", info => 
                {
                    info.getItemDef = () => new()
                    {
                        MajorItem = false,
                        Name = "Mapper_Key",
                        Pool = "Key",
                        PriceCap = 500
                    };
                });
                rb.AddLocationByName("Mapper_Key");
                rb.EditLocationRequest("Mapper_Key", info =>
                {
                    info.getLocationDef = () => new()
                    {
                        Name = "Mapper_Key",
                        SceneName = SceneNames.Crossroads_33,
                        FlexibleCount = false,
                        AdditionalProgressionPenalty = false
                    };
                });
                if (rb.gs.DuplicateItemSettings.DuplicateUniqueKeys)
                    rb.AddItemByName($"{PlaceholderItem.Prefix}Mapper_Key");
            }
        }

        private static void AddGladeKey(RequestBuilder rb)
        {
            if (!AccessManager.Settings.Enabled || !AccessManager.Settings.GladeAccess)
                return;
            
            rb.AddItemByName("Glade_Key");
            if (rb.gs.DuplicateItemSettings.DuplicateUniqueKeys)
                    rb.AddItemByName($"{PlaceholderItem.Prefix}Glade_Key");
            rb.EditItemRequest("Glade_Key", info => 
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    Name = "Glade_Key",
                    Pool = "Key",
                    PriceCap = 500
                };
            });

            // Rebuild the Seer location to have it remove the Glade's unlocking
            rb.EditLocationRequest(LocationNames.Seer, info =>
            {
                info.customPlacementFetch = (factory, placement) =>
                {
                    AbstractPlacement seer = Seer.GetSeerPlacement(factory);
                    seer.AddTag<DestroySeerRewardTag>().destroyRewards = GetRandomizedSeerRewards(rb.gs);
                    if (factory.TryFetchPlacement(LocationNames.Seer, out AbstractPlacement p)) return p;
                    return seer;
                };
            });
        }
        
        public static SeerRewards GetRandomizedSeerRewards(GenerationSettings gs)
        {
            SeerRewards sr = SeerRewards.GladeDoor;
            if (gs.PoolSettings.Relics)
            {
                sr |= SeerRewards.HallownestSeal | SeerRewards.ArcaneEgg;
            }
            if (gs.PoolSettings.PaleOre)
            {
                sr |= SeerRewards.PaleOre;
            }
            if (gs.PoolSettings.Charms)
            {
                sr |= SeerRewards.DreamWielder;
            }
            if (gs.PoolSettings.VesselFragments)
            {
                sr |= SeerRewards.VesselFragment;
            }
            if (gs.PoolSettings.Skills)
            {
                sr |= SeerRewards.DreamGate | SeerRewards.AwokenDreamNail;
            }
            if (gs.PoolSettings.MaskShards)
            {
                sr |= SeerRewards.MaskShard;
            }
            return sr;
        }

        private static void ReplaceKeys(RequestBuilder rb)
        {
            if (!AccessManager.Settings.Enabled)
                return;

            if (AccessManager.Settings.UniqueKeys && rb.gs.PoolSettings.Keys)
            {
                // Override Extra Rando's Key Ring
                rb.RemoveItemByName("Key_Ring");
                rb.RemoveItemByName($"{PlaceholderItem.Prefix}Key_Ring");

                // Remove keys from pool
                rb.RemoveItemByName(ItemNames.Simple_Key);
                rb.RemoveItemByName($"{PlaceholderItem.Prefix}{ItemNames.Simple_Key}");

                // Replace a key from start
                if (rb.IsAtStart(ItemNames.Simple_Key) || rb.IsAtStart("Key_Ring"))
                {
                    rb.RemoveFromStart(ItemNames.Simple_Key);
                    rb.RemoveFromStart("Key_Ring");
                    int keyID = rb.rng.Next(0, 3);
                    if (keyID == 0)
                    {
                        rb.AddToStart("Graveyard_Key");
                        rb.AddItemByName("Waterways_Key");
                        rb.AddItemByName("Pleasure_Key");
                        rb.AddItemByName("Coffin_Key");
                    }
                    if (keyID == 1)
                    {
                        rb.AddItemByName("Graveyard_Key");
                        rb.AddToStart("Waterways_Key");
                        rb.AddItemByName("Pleasure_Key");
                        rb.AddItemByName("Coffin_Key");
                    }
                    if (keyID == 2)
                    {
                        rb.AddItemByName("Graveyard_Key");
                        rb.AddItemByName("Waterways_Key");
                        rb.AddToStart("Pleasure_Key");
                        rb.AddItemByName("Coffin_Key");
                    }
                    if (keyID == 3)
                    {
                        rb.AddItemByName("Graveyard_Key");
                        rb.AddItemByName("Waterways_Key");
                        rb.AddItemByName("Pleasure_Key");
                        rb.AddToStart("Coffin_Key");
                    }
                }
                else
                {
                    rb.AddItemByName("Graveyard_Key");
                    rb.AddItemByName("Waterways_Key");
                    rb.AddItemByName("Pleasure_Key");
                    rb.AddItemByName("Coffin_Key");
                }

                // Dupe keys
                if (rb.gs.DuplicateItemSettings.DuplicateUniqueKeys)
                {
                    rb.AddItemByName($"{PlaceholderItem.Prefix}Graveyard_Key");
                    rb.AddItemByName($"{PlaceholderItem.Prefix}Waterways_Key");
                    rb.AddItemByName($"{PlaceholderItem.Prefix}Pleasure_Key");
                    rb.AddItemByName($"{PlaceholderItem.Prefix}Coffin_Key");
                }
                foreach (string item in new List<string> {"Graveyard_Key", "Waterways_Key", "Pleasure_Key", "Coffin_Key"})
                {
                    rb.EditItemRequest(item, info =>
                    {
                        info.getItemDef = () => new()
                        {
                            MajorItem = true,
                            Name = item,
                            Pool = "Key",
                            PriceCap = 500
                        };
                    });
                };
            }
        }
    }
}