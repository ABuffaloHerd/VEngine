using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Events;
using VEngine.Items;
using VEngine.Logging;
using VEngine.Scenes.Combat;

namespace VEngine.Objects
{
    public class ControllableGameObject : GameObject, IControllable
    {
        protected Weapon weapon;

        public Stat MP { get; set; } = 10; // default values
        public Stat SP { get; set; } = 5;

        public ControllableGameObject(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            weapon = WeaponRegistry.WoodenSword.Clone() as Weapon;

            if(weapon == null)
            {
                throw new Exception("Major fuck up in controllable game object constructor");
            }
        }

        public Pattern GetRange()
        {
            return weapon.Range;
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

        public override ICollection<ControlBase> GetHudElements()
        {
            var list = base.GetHudElements();

            ProgressBar mpBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = MP.Current / MP.Max,
                Position = (5, 2),
                DisplayText = $"{MP.Current} / {MP.Max}",
                BarColor = Color.Blue
            };
            Label mplabel = new("MP: ")
            {
                Position = (0, 2)
            };
            list.Add(mpBar);
            list.Add(mplabel);

            ProgressBar spBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = SP.Current / SP.Max,
                Position = (5, 4),
                DisplayText = $"{SP.Current} / {SP.Max}",
                DisplayTextColor = Color.Black,
                BarColor = Color.Yellow
            };
            Label spLabel = new("SP: ")
            {
                Position = (0, 4)
            };
            list.Add(spBar);
            list.Add(spLabel);

            return list;
        }

        public override void Attack(IEnumerable<GameObject> targets, Arena arena)
        {
            // instead of doing fuck all like the base class, use the weapon object's attack function
            var ev = weapon.ApplyEffect(targets, this, arena);

            // always trigger OnAttack
            RaiseOnAttack(ev);
        }
    }
}
