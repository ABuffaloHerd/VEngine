using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Events;
using VEngine.Logging;

namespace VEngine.Objects
{
    public class ControllableGameObject : GameObject, IControllable
    {
        public ControllableGameObject(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {

        }

        public ICollection<ControlBase> GetControls()
        {
            List<ControlBase> controlBases = new();
            Button b = new("blah");
            b.Position = (2, 2);
            b.Click += (s, e) =>
            {
                CombatEvent combatEvent = new();
                combatEvent.EventType = CombatEventType.ACTION;
                GameManager.Instance.SendGameEvent(this, combatEvent);
            };
            b.UseKeyboard = false; // stops memory leaks by plugging it up with duct tape

            controlBases.Add(b);

            return controlBases;
        }

        public override void Attack(IEnumerable<GameObject> targets)
        {
            // instead of doing fuck all like the base class, use the weapon object's attack function
            // but for now just use the base attack to see that it works
            base.Attack(targets);
        }
    }
}
