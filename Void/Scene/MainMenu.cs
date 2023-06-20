using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Event;
using Void.UI;

namespace Void.Scene
{
    public class MainMenu : BaseScene
    {
        private ListBoxMenu menu;
        public MainMenu() : base()
        {
            SceneType = SceneType.MENU;

            Dictionary<string, GameEvent> options = new()
            {
                { "Story",              new(EventType.CHANGE_SCENE, new("story")) },
                { "Paradox Simulation", new(EventType.CHANGE_SCENE, new("paradox")) },
                { "Back",               new(EventType.CHANGE_MENU,  new("title")) }
            };
            menu = new(40, 40, options);

            // subscribe to child menu's callback so that we can recieve events.
            menu.Callback += Process;

            Render();
        }

        public override void Render()
        {
            Children.Add(menu);
        }

        protected override void Process(GameEvent e)
        {
            System.Console.WriteLine("Main menu scene has received a game event");
        }
    }
}
