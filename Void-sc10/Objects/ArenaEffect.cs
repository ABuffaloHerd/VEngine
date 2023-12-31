using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;

namespace VEngine.Objects
{
    public class ArenaEffect : Effect
    {
        public Func<IEnumerable<GameObject>, CombatEvent> ApplyEffect { get; private set; }

        public ArenaEffect(string name, string description, int timer, Func<IEnumerable<GameObject>, CombatEvent> func) :
            base(name, description, timer)
        { 
        }

        public override void Apply(GameObject target)
        {
            throw new NotImplementedException();
        }
    }
}
