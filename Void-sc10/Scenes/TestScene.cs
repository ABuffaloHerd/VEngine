using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using VEngine.Events;

namespace VEngine.Scenes
{
    public class TestScene : Scene
    {
        /// <summary>
        /// Sometimes a massive red screen is an indicator of progress
        /// </summary>
        public TestScene() : base()
        {
            Surface.DefaultBackground = Color.Red;

            ControlsConsole con = new(20, 20)
            {
                Position = new(20, 20)
            };

            Button b = new(20)
            {
                Position = new(0, 0),
                Text = "Return to title"
            };
            b.Click += (s, e) =>
            {
                var sceneChangeEvent = EventPool.GetSceneChangeEvent("title");
                RaiseGameEvent(sceneChangeEvent);
                EventPool.Return(sceneChangeEvent);
            };

            con.Controls.Add(b);

            Children.Add(con);
        }

        protected override void ProcessGameEvent(object sender, IGameEvent e)
        {
            if(e is KeyPressedEvent keyEvent)
            {
                switch(keyEvent.Key)
                {
                    case 'q':
                        var sceneChangeEvent = EventPool.GetSceneChangeEvent("title");
                        RaiseGameEvent(sceneChangeEvent);
                        EventPool.Return(sceneChangeEvent);
                        break;
                }
            }
        }
    }
}
