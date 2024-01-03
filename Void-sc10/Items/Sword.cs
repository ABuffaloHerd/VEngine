using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Events;
using VEngine.Objects;
using VEngine.Scenes.Combat;

namespace VEngine.Items
{
    public class Sword : Weapon
    {
        public Pattern AbilityRange { get; private set; }
        public int Cost { get; private set; }
        private Func<Weapon, IEnumerable<GameObject>, GameObject, Arena, CombatEvent>? abilityFunc;
        public bool HasAbility 
        {
            get => abilityFunc != null;
        }
        public Sword(string name, string description, // it would seem the signature got a bit out of hand.
            int damage, Pattern range, 
            Func<Weapon, IEnumerable<GameObject>, GameObject, Arena, CombatEvent> func, 
            Pattern abilityRange, int spCost,
            Func<Weapon, IEnumerable<GameObject>, GameObject, Arena, CombatEvent>? ability) : 
            base(name, description, damage, range, func)
        {
            abilityFunc = ability;
        }


    }
}
