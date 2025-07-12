using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
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
                var gameEvent = EventPool.GetGameEvent();
                gameEvent.AddData("test", "Event testing to send event from scene to gamemanager");
                RaiseGameEvent(gameEvent);
                EventPool.Return(gameEvent);
            };

            Button b2 = new(20)
            {
                Text = "Change Scene",
                Position = new(0, 2)
            };
            b2.Click += (s, e) =>
            {
                var sceneChangeEvent = EventPool.GetSceneChangeEvent("test_scene");
                RaiseGameEvent(sceneChangeEvent);
                EventPool.Return(sceneChangeEvent);
            };

            Button b3 = new(20)
            {
                Text = "Scenario",
                Position = new(0, 3)
            };
            b3.Click += (s, e) =>
            {
                var sceneChangeEvent = EventPool.GetSceneChangeEvent("scenario_select");
                RaiseGameEvent(sceneChangeEvent);
                EventPool.Return(sceneChangeEvent);
            };

            Button b4 = new(20)
            {
                Text = "Arena Layout",
                Position = new(0, 4)
            };
            b4.Click += (s, e) =>
            {
                var sceneChangeEvent = EventPool.GetSceneChangeEvent("arena_layout");
                RaiseGameEvent(sceneChangeEvent);
                EventPool.Return(sceneChangeEvent);
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
