using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Objects;

namespace VEngine.Effects
{
    public interface ISingleTargetEffect
    {
        void Apply(GameObject obj);
    }

    public interface IMultiTargetEffect
    {
        void Apply(IEnumerable<GameObject> objs);
    }

    public interface IStatModifier
    {
        int Poll();
    }

    public abstract class Effect
    {
        public int Timer { get; set; }
        public int DefaultTimer { get; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public bool IsInfinite { get; set; } = false;

        public Effect(string name, string description, int timer)
        {
            Name = name;
            Description = description;
            Timer = timer;
            DefaultTimer = timer;

            // automatically set infinite if timer is < 0
            if (timer < 0)
                IsInfinite = true;
        }

        public void Reset()
        {
            Timer = DefaultTimer;
        }
    }
}
