using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VEngine.Data;
using VEngine.Events;
using VEngine.Logging;

namespace VEngine.Objects
{
    public class GameObject : Entity
    {
        public Stat Speed { get; set; }

        public Stat MoveDist { get; set; }
        public Data.Direction Looking { get; set; }

        private List<Effect> effects;

        public event EventHandler<GameEvent> OnAttack;
        public event EventHandler<GameEvent> OnDamaged;

        /// <summary>
        /// Creates a game object to be used in combat scenarios
        /// </summary>
        /// <param name="appearance">
        /// AnimatedScreenObject with frame data used for the blinking effect. Should be part 
        /// of the character class.
        /// </param>
        /// <param name="zIndex"></param>
        public GameObject(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            effects = new();
            Speed = (Stat)100;
            MoveDist = (Stat)10;
        }

        public void Move(Point dest)
        {
            if (MoveDist <= 0) return;
            Position += dest;
            MoveDist--;
        }

        public virtual void Update()
        {
            foreach(Effect ef in effects)
            {
                ef.Apply(this);
            }
        }

        public virtual void Attack(IEnumerable<GameObject> targets)
        {
            Logger.Report(this, "Attack function triggered");

            // Damage calculation, on attack effects etc
            foreach(GameObject target in targets)
            {
                target.TakeDamage(0, DamageType.NONE);
            }

            // Trigger the on attack event for subscribers to react

            // === Sample code === //
            GameEvent attacked = new();
            attacked.AddData("targets", targets);
            attacked.AddData("damage", 0);
            attacked.AddData("total_damage", 0);

            OnAttack(this, attacked);

        }

        public virtual void TakeDamage(int damage, DamageType type)
        {
            // Damage calculation (defense, res etc)
        }
    }
}
