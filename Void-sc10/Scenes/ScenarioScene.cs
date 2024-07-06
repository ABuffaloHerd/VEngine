using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Logging;
using VEngine.Scenes.Combat;

namespace VEngine.Scenes
{
    public class ScenarioScene : Scene
    {
        ControlsConsole controls;
        public ScenarioScene() : base()
        {
            controls = new(50, 60)
            {
                Position = new(0, 0),
            };
            controls.Surface.DefaultBackground = Color.DarkRed;

            ListBox lb = new(50, 60);
            lb.Items.Add("CombatTest");
            lb.Items.Add("AITest");

            Button b = new("Send it");
            b.Position = (0, 59);
            b.Click += (s, e) =>
            {
                SceneChangeEvent sceneChangeEvent = new("aitest");
                GameManager.Instance.SendGameEvent(this, sceneChangeEvent);
            };

            controls.Controls.Add(lb);
            controls.Controls.Add(b);
            controls.UseKeyboard = false; // if this isn't set to false the console is never disposed of and pressing space triggers this button again.

            Children.Add(controls);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);

            if (!disposed)
            {
                if (disposing)
                {
                    controls.Dispose();
                }

                disposed = true;
            }
        }

        public override string ToString()
        { 
            return "ScenarioScene";
        }
    }
}
