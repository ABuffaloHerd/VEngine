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
            Button b = new(20)
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

            Button b2 = new(20)
            {
                Text = "Change Scene",
                Position = new(0, 2)
            };
            b2.Click += (s, e) =>
            {
                SceneChangeEvent @event = new();
                @event.AddData("change_scene", "test_scene");
                RaiseGameEvent(@event);
            };

            Button b3 = new(20)
            {
                Text = "Scenario",
                Position = new(0, 3)
            };
            b3.Click += (s, e) =>
            {
                SceneChangeEvent @event = new();
                @event.AddData("change_scene", "scenario_select");
                RaiseGameEvent(@event);
            };

            Button b4 = new(20)
            {
                Text = "Arena Layout",
                Position = new(0, 4)
            };
            b4.Click += (s, e) =>
            {
                SceneChangeEvent @event = new();
                @event.AddData("change_scene", "arena_layout");
                RaiseGameEvent(@event);
            };

            Console title = new(8, 2)
            {
                FontSize = new(20, 30),
                Position = new(2, 2)
            };
            title.Print(0, 0, "The Void");
            

            ControlsConsole menu = new(20, 9)
            {
                Position = new(5, 12)
            };
            menu.Controls.Add(b);
            menu.Controls.Add(b2);
            menu.Controls.Add(b3);
            menu.Controls.Add(b4);
            Border.CreateForSurface(menu, "Main Menu");

            // Add the title console to this object's list of screenobjects
            Children.Add(title);
            Children.Add(menu);
        }
    }
}
