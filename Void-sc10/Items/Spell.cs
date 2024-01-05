using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Events;
using VEngine.Objects;
using VEngine.Scenes.Combat;
using static System.Net.Mime.MediaTypeNames;

namespace VEngine.Items
{
    public class Spell : ICombatItem, ICloneable
    {
        public string Name { get ; set; }
        public string Description { get ; set; }
        public int Cost { get; set; }
        public int Damage { get; set; }
        public Pattern Range { get; private set; }

        /// <summary>
        /// In order, this, targets, wielder, arena, output game event
        /// </summary>
        protected Func<Spell, IEnumerable<GameObject>, GameObject, Arena, CombatEvent> attackFunction;

        public Spell(string name, string description, int damage, int cost, Pattern range, Func<Spell, IEnumerable<GameObject>, GameObject, Arena, CombatEvent> func)
        {
            Name = name;
            Description = description;
            Damage = damage;
            Cost = cost;
            Range = range;
            attackFunction = func;
        }
        public CombatEvent ApplyEffect(IEnumerable<GameObject> targets, GameObject wielder, Arena arena)
        {
            return attackFunction(this, targets, wielder, arena);
        }

        public virtual object Clone()
        {
            return new Spell(
                Name,
                Description,
                Damage,
                Cost,
                Range,
                attackFunction
            );
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
