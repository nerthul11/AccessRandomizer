using AccessRandomizer.IC;
using ItemChanger;
using Newtonsoft.Json;
using RandomizerMod.RC;
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
            RequestBuilder.OnUpdate.Subscribe(1100f, AddObjects);
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
        }

        public static void AddObjects(RequestBuilder builder)
        {
            if (!AccessManager.Settings.Enabled)
                return;
            
            if (AccessManager.Settings.MantisRespect)
            {
                builder.AddItemByName("Mantis_Respect");
                builder.EditItemRequest("Mantis_Respect", info => 
                {
                    info.getItemDef = () => new()
                    {
                        MajorItem = false,
                        Name = "Mantis_Respect",
                        Pool = "Key",
                        PriceCap = 500
                    };
                });
                builder.AddLocationByName("Mantis_Respect");
                builder.EditLocationRequest("Mantis_Respect", info =>
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
                builder.AddItemByName("Hollow_Knight_Chain", 4);
                if (builder.gs.DuplicateItemSettings.Dreamer)
                    builder.AddItemByName($"{PlaceholderItem.Prefix}Hollow_Knight_Chain", 2);
                builder.EditItemRequest("Hollow_Knight_Chain", info => 
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
                    builder.AddLocationByName($"Hollow_Knight_Chain-{i}");
                    builder.EditLocationRequest($"Hollow_Knight_Chain-{i}", info =>
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

            if (AccessManager.Settings.UniqueKeys && builder.gs.PoolSettings.Keys)
            {
                // Override Extra Rando's Key Ring
                builder.RemoveItemByName("Key_Ring");
                builder.RemoveItemByName($"{PlaceholderItem.Prefix}Key_Ring");

                // Remove keys from pool
                builder.RemoveItemByName(ItemNames.Simple_Key);
                builder.RemoveItemByName($"{PlaceholderItem.Prefix}{ItemNames.Simple_Key}");

                // Replace a key from start
                if (builder.IsAtStart(ItemNames.Simple_Key) || builder.IsAtStart("Key_Ring"))
                {
                    builder.RemoveFromStart(ItemNames.Simple_Key);
                    builder.RemoveFromStart("Key_Ring");
                    int keyID = builder.rng.Next(0, 3);
                    if (keyID == 0)
                    {
                        builder.AddToStart("Graveyard_Key");
                        builder.AddItemByName("Waterways_Key");
                        builder.AddItemByName("Pleasure_Key");
                        builder.AddItemByName("Coffin_Key");
                    }
                    if (keyID == 1)
                    {
                        builder.AddItemByName("Graveyard_Key");
                        builder.AddToStart("Waterways_Key");
                        builder.AddItemByName("Pleasure_Key");
                        builder.AddItemByName("Coffin_Key");
                    }
                    if (keyID == 2)
                    {
                        builder.AddItemByName("Graveyard_Key");
                        builder.AddItemByName("Waterways_Key");
                        builder.AddToStart("Pleasure_Key");
                        builder.AddItemByName("Coffin_Key");
                    }
                    if (keyID == 3)
                    {
                        builder.AddItemByName("Graveyard_Key");
                        builder.AddItemByName("Waterways_Key");
                        builder.AddItemByName("Pleasure_Key");
                        builder.AddToStart("Coffin_Key");
                    }
                }
                else
                {
                    builder.AddItemByName("Graveyard_Key");
                    builder.AddItemByName("Waterways_Key");
                    builder.AddItemByName("Pleasure_Key");
                    builder.AddItemByName("Coffin_Key");
                }

                // Dupe keys
                if (builder.gs.DuplicateItemSettings.DuplicateUniqueKeys)
                {
                    builder.AddItemByName($"{PlaceholderItem.Prefix}Graveyard_Key");
                    builder.AddItemByName($"{PlaceholderItem.Prefix}Waterways_Key");
                    builder.AddItemByName($"{PlaceholderItem.Prefix}Pleasure_Key");
                    builder.AddItemByName($"{PlaceholderItem.Prefix}Coffin_Key");
                }
                foreach (string item in new List<string> {"Graveyard_Key", "Waterways_Key", "Pleasure_Key", "Coffin_Key"})
                {
                    builder.EditItemRequest(item, info =>
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