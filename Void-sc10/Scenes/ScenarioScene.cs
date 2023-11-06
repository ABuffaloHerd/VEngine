using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Scenes
{
    public class ScenarioScene : Scene
    {
        public ScenarioScene() : base()
        {
            ControlsConsole controls = new(50, 60)
            {
                Position = new(0, 0),
            };
            controls.Surface.DefaultBackground = Color.DarkRed;

            ListBox lb = new(50, 60);
            lb.Items.Add("Item1");
            lb.Items.Add("Item2");
            lb.Items.Add("Item3");
            lb.Items.Add("Item4");
        }
    }
}
