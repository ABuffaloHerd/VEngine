using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Void;
using SadConsole;
using SadConsole.Entities;
using Void.Item.Weapon;

namespace Void.Battle
{
    public abstract class GameObject : Entity
    {
        new public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }
        
        public GameObject(ColoredGlyph glyph) : base(glyph, 10)
        {
            Position = new(0, 0);
        }
    }

    public class MeleeCharacter : GameObject
    {
        public int Mana { get; protected set; }
        public int MaxMana { get; protected set; }

        public BaseWeapon Weapon { get; protected set; }

        public MeleeCharacter(ColoredGlyph glyph) : base(glyph)
        {

        }
    }

    public class TestObject : GameObject
    {
        public TestObject(ColoredGlyph coloredGlyph) : base(coloredGlyph)
        {

        }
    }
}
