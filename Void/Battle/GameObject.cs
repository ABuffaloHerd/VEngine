using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Void;
using SadConsole;
using SadConsole.Entities;
using Void.Item.Weapon;
using Void.DataStructures;

namespace Void.Battle
{
    public enum Alignment
    {
        PLAYER,
        ENEMY,
        NEUTRAL
    }
    public abstract class GameObject : Entity
    {
        public Alignment Alignment { get; set; }
        public Guid ID { get; protected set; }
        new public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }
        public GameObject(Color foreground, Color background, int glyph) : base(foreground: foreground, background: background, glyph: glyph, zIndex: 0)
        {
            Position = new(0, 0);
            ID = Guid.NewGuid();
            AnimatedAppearanceComponent animatedAppearanceComponent = null;
            Alignment = Alignment.NEUTRAL;
        }

        public abstract Pattern GetRange();
    }

    //public class MeleeCharacter : GameObject
    //{
    //    public int Mana { get; protected set; }
    //    public int MaxMana { get; protected set; }

    //    public BaseWeapon Weapon { get; protected set; }
    //}

    public class TestObject : GameObject
    {
        public PlayerWeapon Weapon { get; protected set; }
        public TestObject(char c) : base(Color.White, Color.Black, c)
        {
            Alignment = Alignment.PLAYER;
            Pattern p = new Pattern();
            p.Mark(0, 0);
            p.Mark(1, 0);
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
