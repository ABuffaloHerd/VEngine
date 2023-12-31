using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;

namespace VEngine.Objects
{
    public abstract class Effect
    {
        public int Timer { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public Effect(string name, string description, int timer)
        {
            Timer = timer;
        }

        public abstract void Apply(GameObject target);  
    }

    public class EntityEffect : Effect
    {
        public Func<GameObject, CombatEvent> ApplyEffect { get; private set; }
        public EntityEffect(string name, string description, int timer, Func<GameObject, CombatEvent> applyEffect) : base(name, description, timer)
        {
            ApplyEffect = applyEffect;
        }

        public override void Apply(GameObject target)
        {
            ApplyEffect(target);
            Timer--;
        }
    }
}
