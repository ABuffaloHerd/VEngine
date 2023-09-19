using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Battle;
using Void.DataStructures;

namespace Void.Item.Weapon
{
    public abstract class BaseWeapon
    {
        public string Name { get; protected set; }
        public int Damage { get; protected set; }

        /* The attack function takes in a list of gameobjects and returns the total damage dealt.
         * The list of gameobjects are all the valid targets for this weapon.
         * If it is a single target weapon, then the list should contain only one gameobject 
         */
        public Func<List<GameObject>, int> AttackFunction { get; protected set; }
        public BaseWeapon(string name, int damage, Func<List<GameObject>, int> atk)
        {
            Name = name;
            Damage = damage;
            AttackFunction = atk;
        }
    }

    public abstract class PlayerWeapon : BaseWeapon
    {   
        public Pattern Range { get; private set; }
        public PlayerWeapon(string name, int damage, Func<List<GameObject>, int> atk, Pattern range) : base(name, damage, atk)
        {
            Range = range;
        }
    }

    public class MeleeWeapon : PlayerWeapon
    { 
        public MeleeWeapon(string name, int damage, Func<List<GameObject>, int> atk, Pattern range) : base(name, damage, atk, range)
        {
        }
    }
}
