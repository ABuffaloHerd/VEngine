using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Battle;

namespace Void.Item.Weapon
{
    public abstract class BaseWeapon
    {
        public string Name { get; protected set; }
        public int Damage { get; protected set; }

        public Func<List<GameObject>, int> AttackFunction { get; protected set; }
        public BaseWeapon(string name, int damage, Func<List<GameObject>, int> atk)
        {
            Name = name;
            Damage = damage;
            AttackFunction = atk;
        }
    }
}
