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
                System.Console.WriteLine("click");
                GameEvent @event = new();
                @event.AddData("test", "Event testing to send event from scene to gamemanager");
                RaiseGameEvent(@event);
            };

            ControlsConsole title = new(8, 2)
            {
                FontSize = new(15, 20),
                Position = new(2, 2)
            };
            title.Print(0, 0, "The Void");
            title.Controls.Add(b);

            // Add the title console to this object's list of screenobjects
            Children.Add(title);

            // Tell the game manager that this is its current scene
            gmInstance.ChangeScene(this);
        }
    }
}
