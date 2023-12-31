using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Events;
using VEngine.Factory;
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

            // set attributes for MP bar
            MP.IsOverloadable = true;
            MP.Current = 10;
        }

        public Pattern GetRange()
        {
            return weapon.Range;
        }

        public virtual ICollection<ControlBase> GetControls()
        {
            List<ControlBase> controlBases = new();
            Button b = new("blah");
            b.Position = (2, 2);
            b.UseKeyboard = false; // stops memory leaks by plugging it up with duct tape

            controlBases.Add(b);

            PlugMemoryLeaks(controlBases);
            return controlBases;
        }

        public override ICollection<ControlBase> GetHudElements()
        {
            var list = base.GetHudElements();

            ProgressBar mpBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = MP.Current / (float)MP.Max,
                Position = (5, 2),
                DisplayText = (MP.Overloaded ? "Overload! " : "") + $"{MP.Current} / {MP.Max}",
                BarColor = MP.Overloaded ? Color.Purple : Color.Blue,
            };
            Label mplabel = new("MP: ")
            {
                Position = (0, 2)
            };
            list.Add(mpBar);
            list.Add(mplabel);

            ProgressBar spBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = SP.Current / (float)SP.Max,
                Position = (5, 4),
                DisplayText = $"{SP.Current} / {SP.Max}",
                DisplayTextColor = Color.Black,
                BarColor = Color.Yellow,
                BackgroundGlyph = 178
            };
            Label spLabel = new("SP: ")
            {
                Position = (0, 4)
            };
            list.Add(spBar);
            list.Add(spLabel);


            PlugMemoryLeaks(list);
            return list;
        }

        public override void Attack(IEnumerable<GameObject> targets, Arena arena)
        {
            // instead of doing fuck all like the base class, use the weapon object's attack function
            var ev = weapon.ApplyEffect(targets, this, arena);

            // always trigger OnAttack
            RaiseOnAttack(ev);
        }

        public override void Cast(IEnumerable<GameObject> targets, Arena arena, Spell spell)
        {
            if (MP - spell.Cost < 0) return; // check for mp

            MP -= spell.Cost;
            var ev = spell.ApplyEffect(targets, this, arena);
            RaiseOnAttack(ev);
        }
    }
}
