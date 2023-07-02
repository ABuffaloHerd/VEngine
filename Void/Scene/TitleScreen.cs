using SadConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Event;
using Void.UI;

namespace Void.Scene
{
    public class TitleScreen : BaseScene
    {
        private Menu mainMenu;
        public TitleScreen()
        {
            SceneType = SceneType.MENU;
            Render();
        }
        public override void Render()
        {
            // setup main menu
            Console title = new(8, 1)
            {
                FontSize = new(15, 25),
                Position = new(2, 2),
                UseMouse = false
            };
            title.Print(0, 0, "The Void");

            Children.Add(title);

            Dictionary<string, GameEvent> menuOptions = new();
            menuOptions.Add("Start", new(EventType.NEW_GAME, new("newgame")));
            menuOptions.Add("Options", new(EventType.LOAD_GAME, new("loadgame")));
            menuOptions.Add("Debug", new(EventType.DEBUG, new("debug_scene")));
            menuOptions.Add("Quit", new(EventType.DEBUG, new()));

            mainMenu = new(11, 4, menuOptions, "Main Menu");
            mainMenu.Position = new(5, 7);
            mainMenu.Render();

            Border.BorderParameters bp = Border.BorderParameters.GetDefault();
            new Border(mainMenu, bp);

            Children.Add(mainMenu);
        }
    }
}
