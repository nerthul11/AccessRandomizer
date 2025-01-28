using System;
using AccessRandomizer.Manager;
using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod.Menu;
using Satchel.BetterMenus;
using UnityEngine;

namespace AccessRandomizer.Settings
{
    public class ConnectionMenu 
    {
        // Top-level definitions
        internal static ConnectionMenu Instance { get; private set; }
        private readonly SmallButton pageRootButton;

        // Menu page and elements
        private readonly MenuPage accessPage;
        private MenuElementFactory<AccessSettings> topLevelElementFactory;

        // Custom Keys menu
        private SmallButton ckPageButton;
        internal MenuElementFactory<CustomKeySettings> customKeyMEF;

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
            accessPage = new MenuPage("accessPage", connectionPage);
            
            topLevelElementFactory = new(accessPage, AccessManager.Settings);
            VerticalItemPanel topLevelPanel = new(accessPage, new Vector2(0, 400), 60, true, topLevelElementFactory.Elements); 
            ckPageButton = new(accessPage, "Custom Keys");
            ckPageButton.AddHideAndShowEvent(DisplayCK());
            topLevelPanel.Add(ckPageButton);
            topLevelElementFactory.ElementLookup[nameof(AccessSettings.Enabled)].SelfChanged += EnableSwitch;
            topLevelPanel.ResetNavigation();
            topLevelPanel.SymSetNeighbor(Neighbor.Down, accessPage.backButton);
            topLevelPanel.SymSetNeighbor(Neighbor.Up, accessPage.backButton);
            pageRootButton = new SmallButton(connectionPage, "Access Randomizer");
            pageRootButton.AddHideAndShowEvent(connectionPage, accessPage);
        }
        // Add Custom Keys page
        private MenuPage DisplayCK() {
            MenuPage ckPage = new("Custom Keys", accessPage);
            MenuLabel header = new(ckPage, "Custom Keys");
            customKeyMEF = new(ckPage, AccessManager.Settings.CustomKeys);
            VerticalItemPanel ckPanel = new(ckPage, new Vector2(0, 400), 60, false);
            ckPanel.Add(header);
            foreach (IValueElement element in customKeyMEF.Elements)
                ckPanel.Add(element);
            ckPanel.ResetNavigation();
            ckPanel.SymSetNeighbor(Neighbor.Up, ckPage.backButton);
            ckPanel.SymSetNeighbor(Neighbor.Down, ckPage.backButton);
            SetButtonColor(ckPageButton, () => AccessManager.Settings.CustomKeys.Any());
            return ckPage;
        }
        // Define parameter changes
        private void EnableSwitch(IValueElement obj)
        {
            pageRootButton.Text.color = AccessManager.Settings.Enabled ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
        }

        private void SetButtonColor(SmallButton target, Func<bool> condition)
        {
            target.Parent.BeforeShow += () =>
            {
                target.Text.color = condition() ? Colors.TRUE_COLOR : Colors.FALSE_COLOR;
            };
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
            customKeyMEF.SetMenuValues(settings.CustomKeys);      
        }
    }
}