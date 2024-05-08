using AccessRandomizer.Manager;
using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod.Menu;
using UnityEngine;

namespace AccessRandomizer.Settings
{
    public class ConnectionMenu 
    {
        // Top-level definitions
        internal static ConnectionMenu Instance { get; private set; }
        private readonly SmallButton pageRootButton;

        // Menu page and elements
        private readonly MenuPage ghPage;
        private MenuElementFactory<AccessSettings> topLevelElementFactory;

        public static void Hook()
        {
            RandomizerMenuAPI.AddMenuPage(ConstructMenu, HandleButton);
            MenuChangerMod.OnExitMainMenu += () => Instance = null;
        }

        private static bool HandleButton(MenuPage landingPage, out SmallButton button)
        {
            button = Instance.pageRootButton;
            button.Text.color = AccessManager.Settings.Enabled ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
            return true;
        }

        private static void ConstructMenu(MenuPage connectionPage)
        {
            Instance = new(connectionPage);
        }

        private ConnectionMenu(MenuPage connectionPage)
        {
            // Define connection page
            ghPage = new MenuPage("ghPage", connectionPage);
            topLevelElementFactory = new(ghPage, AccessManager.Settings);
            topLevelElementFactory.ElementLookup["Enabled"].SelfChanged += EnableSwitch;
            VerticalItemPanel topLevelPanel = new(ghPage, new Vector2(0, 400), 60, true);        
            topLevelPanel.Add(topLevelElementFactory.ElementLookup["Enabled"]);
            topLevelPanel.Add(topLevelElementFactory.ElementLookup["MantisRespect"]);
            topLevelPanel.Add(topLevelElementFactory.ElementLookup["HollowKnightChains"]);
            topLevelPanel.Add(topLevelElementFactory.ElementLookup["UniqueKeys"]);
            topLevelPanel.ResetNavigation();
            topLevelPanel.SymSetNeighbor(Neighbor.Down, ghPage.backButton);
            pageRootButton = new SmallButton(connectionPage, "Access Randomizer");
            pageRootButton.AddHideAndShowEvent(connectionPage, ghPage);
        }
        // Define parameter changes
        private void EnableSwitch(IValueElement obj)
        {
            pageRootButton.Text.color = AccessManager.Settings.Enabled ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
        }

        // Apply proxy settings
        public void Disable()
        {
            IValueElement elem = topLevelElementFactory.ElementLookup[nameof(AccessSettings.Enabled)];
            elem.SetValue(false);
        }

        public void Apply(AccessSettings settings)
        {
            topLevelElementFactory.SetMenuValues(settings);        
        }
    }
}