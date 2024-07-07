using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Events;
using VEngine.Logging;
using VEngine.Objects;
using VEngine.Scenes.Combat;

namespace VEngine.Items
{
    public class Weapon : ICombatItem, ICloneable
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int Damage { get; set; }

        public Pattern Range { get; private set; }

        /// <summary>
        /// Holds unique weapon data
        /// </summary>
        private Dictionary<string, object> data = new();

        /// <summary>
        /// In order, this, targets, wielder, arena, output game event
        /// </summary>
        protected Func<Weapon, IEnumerable<GameObject>, GameObject, Arena, CombatEvent> attackFunction;

        /// <summary>
        /// Creates a new weapon class
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="func">Lambda expression for weapon's actions</param>
        public Weapon(string name, string description, int damage, Pattern range, Func<Weapon, IEnumerable<GameObject>, GameObject, Arena, CombatEvent> func)
        {
            Name = name;
            Description = description;
            Damage = damage;
            Range = range;
            attackFunction = func;
        }

        public CombatEvent ApplyEffect(IEnumerable<GameObject> targets, GameObject wielder, Arena arena)
        {
            CombatEvent ev = attackFunction(this, targets, wielder, arena);
            return ev;
        }

        public virtual object Clone()
        {
            return new Weapon(
                Name,
                Description,
                Damage,
                Range,
                attackFunction
            );
        }
    }

    public class RangedWeapon : Weapon
    {
        public int MaxAmmo { get; private set; }
        public RangedWeapon(string name, string description, int damage, int maxammo, Pattern range, Func<Weapon, IEnumerable<GameObject>, GameObject, Arena, CombatEvent> func) :
            base(name, description, damage, range, func)
        {
            MaxAmmo = maxammo;
        }

        public override object Clone()
        {
            return new RangedWeapon(
                Name,
                Description,
                Damage,
                MaxAmmo,
                Range,
                attackFunction
            );
        }
    }
}
