using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;

namespace VEngine.Scenes
{
    public class TitleScene : Scene
    {
        public TitleScene()
        {
            Button b = new(6)
            {
                Text = "qwerty",
                Position = new(0, 1)
            };
            b.Click += (s, e) =>
            {
                GameEvent @event = new();
                @event.AddData("test", "Event testing to send event from scene to gamemanager");
                RaiseGameEvent(@event);
            };

            Console title = new(8, 2)
            {
                FontSize = new(20, 30),
                Position = new(2, 2)
            };
            title.Print(0, 0, "The Void");

            Console menu = new(20, 9)
            {
                Position = new(5, 12)
            };
            menu.Print(0, 0, "Sample text");

            // Add the title console to this object's list of screenobjects
            Children.Add(title);
            Children.Add(menu);
        }
    }
}
