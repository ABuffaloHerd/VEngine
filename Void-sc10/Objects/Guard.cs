using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Items;

namespace VEngine.Objects
{
    public class Guard : ControllableGameObject
    {
        public Guard(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            // Test code
            weapon = (Weapon)WeaponRegistry.CombatSword.Clone();
        }
    }
}
