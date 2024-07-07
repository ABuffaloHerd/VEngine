using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Items;
using VEngine.Objects;

namespace VEngine.Components
{
    public abstract class Component
    {
        public GameObject parent;
    }

    public class CollisionComponent : Component
    {
        public int Weight = 1;
    }
    
    public class WeaponComponent : Component
    {
        public Weapon Weapon = WeaponRegistry.Hands; // default
        public Pattern Range => Weapon.Range;
    }

    public class OverdriveComponent : Component
    {
        public Stat Stat { get; set; } = new(0, 100);
        public OverdriveComponent() 
        {
        }
    }
}
