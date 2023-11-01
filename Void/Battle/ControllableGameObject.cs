using SadConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.DataStructures;
using Void.Item.Weapon;

namespace Void.Battle
{
    public abstract class ControllableGameObject : GameObject, IControllable, IMovable
    {
        protected CharacterControls Controls;

        public ControllableGameObject(Animated appearance) : base(appearance)
        {
            Controls = new(14, 14, "test controls");
        }

        public ControllableGameObject(Color fg, Color bg, int glyph) : base(fg, bg, glyph)
        {
            Controls = new(14, 14, "test controls");
        }

        public ControlsConsole GetControls()
        {
            return Controls;
        }

        public virtual void Backdash(Point delta)
        {
            this.Position.Add(delta);
        }
        public virtual void Move(Point delta)
        {
            this.Position.Add(delta);
        }

    }

    public class TestControllableGameObject : ControllableGameObject
    {
        public PlayerWeapon Weapon { get; protected set; }
        public TestControllableGameObject(char c) : base(Color.White, Color.Black, c)
        {
            Name = "test object";
            Alignment = Alignment.PLAYER;
            Pattern p = new Pattern();
            p.Mark(0, 0);
            p.Mark(1, 0);
            p.Mark(2, 0);
            p.Mark(3, 0);
            p.Mark(0, 1);
            p.Mark(0, -1);
            p.Mark(1, 1);
            p.Mark(1, -1);
            Weapon = new MeleeWeapon(
                "test sword",
                10,
                (targets) =>
                {
                    int totalDamage = 0;
                    foreach (GameObject target in targets)
                    {
                        totalDamage += 10;
                    }
                    return totalDamage;
                },
                p
                );
        }

        public override Pattern GetRange()
        {
            return Weapon.Range;
        }
    }
}
