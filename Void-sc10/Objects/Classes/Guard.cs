using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Components;
using VEngine.Items;
using VEngine.Logging;

namespace VEngine.Objects.Classes
{
    // Melee damage machine
    public class Guard : ControllableGameObject
    {
        public Guard(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            // Test code
            Weapon = (Weapon)WeaponRegistry.CombatSword.Clone();

            AddComponent(new OverdriveComponent());
        }
    }
}
