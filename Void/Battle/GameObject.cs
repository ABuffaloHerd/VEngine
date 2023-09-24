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
using System.ComponentModel.DataAnnotations;
using SadConsole.Effects;

namespace Void.Battle
{
    public enum Alignment
    {
        PLAYER,
        ENEMY,
        NEUTRAL
    }
    public abstract partial class GameObject : Entity
    {
        public Alignment Alignment { get; set; }
        public Guid ID { get; protected set; }
        new public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }
        public int Speed { get; set; }

        private static AnimatedScreenObject PreProcess(Color foreground, Color background, int glyph, int sizeX, int sizeY)
        {
            var returnme = new AnimatedScreenObject("entity", sizeX, sizeY);

            var frame = returnme.CreateFrame();
            frame[0].Foreground = foreground; 
            frame[0].Background = background;
            frame[0].Glyph = glyph;
            returnme.Repeat = true;

            return returnme;
        }
        public GameObject(Color foreground, Color background, int glyph) : base(PreProcess(foreground, background, glyph, 1, 1), zIndex: 0)
        {
            Position = new(0, 0);
            ID = Guid.NewGuid();
            Alignment = Alignment.NEUTRAL;
            Speed = 100;
        }

        public GameObject SetPosition(int x, int y)
        {
            Position = new(x, y);

            return this;
        }

        public GameObject SetVisualEffect(SadConsole.Effects.ICellEffect effect)
        {
            System.Console.WriteLine("This object should be blinking");
            return this;
        }

        public override string ToString()
        {
            return Name;
        }

        public void SetBlinker(bool state)
        {
            // if true, change this cell's appearance to an animated blinking one
            if (state)
            {

            }
        }

        public abstract Pattern GetRange();

        public class SpeedComparer : IComparer<GameObject>
        {
            public int Compare(GameObject x, GameObject y)
            {
                return x!.Speed - y!.Speed;
            }
        }
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
            Name = "test object";
            Alignment = Alignment.PLAYER;
            Pattern p = new Pattern();
            p.Mark(0, 0);
            p.Mark(1, 0);
            p.Mark(2, 0);
            p.Mark(3, 0);
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
