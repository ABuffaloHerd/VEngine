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
                SceneChangeEvent @event = new("title");
                RaiseGameEvent(@event);
            };

            con.Controls.Add(b);

            Children.Add(con);
        }

        protected override void ProcessGameEvent(object sender, IGameEvent e)
        {
            if(e is KeyPressedEvent)
            {
                KeyPressedEvent kp = e as KeyPressedEvent;

                switch(kp.Key)
                {
                    case 'q':
                        SceneChangeEvent ev = new("title");
                        RaiseGameEvent(ev);
                        break;
                }
            }
        }
    }
}
