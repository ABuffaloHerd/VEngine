using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Objects;

namespace VEngine.Effects
{
    public class ArenaEffect : Effect, IMultiTargetEffect
    {
        // TODO: See if the combat event is required
        public Func<IEnumerable<GameObject>, CombatEvent> ApplyEffect { get; private set; }

        public ArenaEffect(string name, string description, int timer, Func<IEnumerable<GameObject>, CombatEvent> func) :
            base(name, description, timer)
        {
            ApplyEffect = func;
        }

        public void Apply(IEnumerable<GameObject> objs)
        {
            ApplyEffect(objs);

            if (!IsInfinite)
                Timer--;
        }
    }
}
