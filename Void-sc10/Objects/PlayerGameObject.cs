using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Logging;

namespace VEngine.Objects
{
    public class PlayerGameObject : GameObject, IControllable
    {
        public PlayerGameObject(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {

        }

        public ICollection<ControlBase> GetControls()
        {
            List<ControlBase> controlBases = new();
            Button b = new("blah");
            b.Position = (2, 2);
            b.Click += (s, e) =>
            {
                GameManager.Instance.SendGameEvent(this,
                    new GameEvent("controllable game object button clicked"));
            };
            b.UseKeyboard = false; // stops memory leaks by plugging it up with duct tape

            controlBases.Add(b);

            return controlBases;
        }
    }
}
