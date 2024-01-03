using VEngine.Events;
using VEngine.Objects;

namespace VEngine.Effects
{
    public class EntityEffect : Effect, ISingleTargetEffect
    {
        public Func<GameObject, CombatEvent> ApplyEffect { get; private set; }
        public EntityEffect(string name, string description, int timer, Func<GameObject, CombatEvent> applyEffect) : base(name, description, timer)
        {
            ApplyEffect = applyEffect;
        }

        public  void Apply(GameObject target)
        {
            ApplyEffect(target);
            if (!IsInfinite)
                Timer--;
        }
    }
}
