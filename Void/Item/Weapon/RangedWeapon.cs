using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Battle;

namespace Void.Item.Weapon
{
    public class RangedWeapon : BaseWeapon
    {
        public int MaxAmmo { get; protected set; }
        public int Ammo { get; protected set; }
        public RangedWeapon(string name, int damage, Func<List<GameObject>, int> atk) : base(name, damage, atk)
        {
        }
    }
}
