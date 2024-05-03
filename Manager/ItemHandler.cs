using ItemChanger;
using RandomizerMod.RC;
using AccessRandomizer.IC;
using System.Linq;

namespace AccessRandomizer.Manager {
    internal static class ItemHandler
    {
        internal static void Hook()
        {
            DefineObjects();
            RequestBuilder.OnUpdate.Subscribe(1f, AddObjects);
        }

        public static void DefineObjects()
        {
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
            }

            if (AccessManager.Settings.HollowKnightChains)
            {
                builder.AddItemByName("Hollow_Knight_Chain", 4);
                builder.EditItemRequest("Hollow_Knight_Chain", info => 
                {
                    info.getItemDef = () => new()
                    {
                        MajorItem = true,
                        Name = "Hollow_Knight_Chain",
                        Pool = "Key",
                        PriceCap = 2000
                    };
                });
                foreach (int i in Enumerable.Range(1, 4))
                {
                    builder.AddLocationByName($"Hollow_Knight_Chain-{i}");
                }
            }
        }
    }
}