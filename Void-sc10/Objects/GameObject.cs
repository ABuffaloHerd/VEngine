using SadConsole.Entities;
using SadConsole.UI;
using SadConsole.UI.Controls;
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
        public Stat HP { get; set; }
        public Stat Speed { get; set; }

        public Stat MoveDist { get; set; }
        public Data.Direction Looking { get; set; }

        private List<Effect> effects;

        public event EventHandler<GameEvent> OnAttack;

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
            HP = 10; // default debug
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
                target.TakeDamage(1, DamageType.NONE);
            }

            // Trigger the on attack event for subscribers to react

            // === Sample code === //
            GameEvent attacked = new();
            attacked.AddData("targets", targets);
            attacked.AddData("damage", 1);
            attacked.AddData("total_damage", 1);

            OnAttack(this, attacked);

        }

        public virtual void TakeDamage(int damage, DamageType type)
        {
            // Damage calculation (defense, res etc)
            HP.Current -= damage;

            CombatEvent damaged = new();
            damaged.AddData("amount", damage);
            damaged.AddData("me", this);
            damaged.Target = EventTarget.CURRENT_SCENE;

            Logger.Report(this, $"Took {damage} damage. HP: {HP.Current} / {HP.Max}");

            GameManager.Instance.SendGameEvent(this, damaged);
        }

        /// <summary>
        /// Builds a collection of control bases containing hud elements
        /// based off current object's stats
        /// </summary>
        /// <returns>Collection of these control bases</returns>
        public virtual ICollection<ControlBase> GetHudElements()
        {
            List<ControlBase> list = new();

            ProgressBar hpBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = (float)HP.Current / (float)HP.Max,
                Position = new(5, 0)
            };
            hpBar.BarColor = Color.Red;

            list.Add(hpBar);

            return list;
        }
    }
}
