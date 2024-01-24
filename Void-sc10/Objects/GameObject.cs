using SadConsole.Effects;
using SadConsole.Entities;
using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VEngine.Data;
using VEngine.Effects;
using VEngine.Events;
using VEngine.Factory;
using VEngine.Items;
using VEngine.Logging;
using VEngine.Scenes.Combat;

namespace VEngine.Objects
{
    public class GameObject : Entity
    {
        public Stat HP { get; set; }
        public Stat Speed { get; set; }
        public Stat MoveDist { get; set; }
        public Stat DEF { get; set; }
        public Stat RES { get; set; }

        public Alignment Alignment { get; set; } = Alignment.FRIEND;

        /// <summary>
        /// Does this do anything apart from exist?
        /// </summary>
        public bool IsStatic { get; set; } = false;

        /// <summary>
        /// Can i walk into this?
        /// </summary>
        public bool HasCollision { get; set; } = true;

        /// <summary>
        /// Take a guess. Is this potato a bomb?
        /// </summary>
        public Type Type { get; set; } = Type.ENTITY; 

        /// <summary>
        /// Get your head out of your six.
        /// </summary>
        public Data.Direction Facing 
        {
            get => facing;
            set
            {
                if (facing == value) return;
                Data.Direction old = facing;
                facing = value;
                OnDirectionChanged(old, value);
            }

        }
        protected Data.Direction facing = Data.Direction.RIGHT;

        public virtual Pattern Range => null;

        protected List<Effect> effects;

        protected EffectsManager effectsManager;

        public event EventHandler<GameEvent>? OnAttack;
        public event EventHandler<GameEvent>? OnSpellCast;

        /// <summary>
        /// Triggered when Facing is changed
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<Data.Direction>>? DirectionChanged;

        public bool IsDead
        {
            get => HP.Current <= 0;
        }

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

            // defaults
            DEF = 0;
            RES = 0;
        }

        public virtual void Move(Point dest)
        {
            if (MoveDist <= 0) return;
            Position += dest;
            MoveDist--;
        }

        public virtual void Update()
        {
            foreach(EntityEffect ef in effects)
            {
                ef.Apply(this);
            }
        }

        /// <summary>
        /// Should run when this object starts its turn
        /// </summary>
        public virtual void OnStartTurn()
        {
            Blink();
        }

        /// <summary>
        /// Function that actually executes the attack. It is the one responsible for sending the combat event to the game manager via OnAttack.Invoke
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="arena"></param>
        public virtual void Attack(IEnumerable<GameObject> targets, Arena arena)
        {
            Logger.Report(this, "Attack function triggered");

            // Damage calculation, on attack effects etc

            /// === TESTING CODE === ///
            foreach(GameObject target in targets)
            {
                target.TakeDamage(this, null, 1, DamageType.NONE);
            }

            // Trigger the on attack event for subscribers to react

            // === Sample event === //
            GameEvent attacked = new();
            attacked.AddData("targets", targets);
            attacked.AddData("damage", 1);
            attacked.AddData("total_damage", 1);

            // This is always called
            OnAttack?.Invoke(this, attacked);
        }

        public virtual void Cast(IEnumerable<GameObject> targets, Arena arena, Spell spell)
        {

        }

        /// <summary>
        /// Performs damage calculations and the actual reduction of HP
        /// </summary>
        /// <param name="damage">Amount of pain</param>
        /// <param name="type">Style of pain</param>
        /// <returns>Amount of pain taken after painkillers</returns>
        public virtual int TakeDamage(GameObject? attacker, ICombatItem? item, int damage, DamageType type)
        {
            int taken;
            // Damage calculation (defense, res etc)
            switch(type)
            {
                case DamageType.MAGIC:
                    // reduce by RES%
                    if (RES.Current != 0)
                        taken = (int)Math.Floor(damage * (RES.Current / 100f));
                    else
                        taken = damage;
                    break;

                case DamageType.PHYSICAL:
                    // reduce by DEF
                    taken = damage - DEF.Current;
                    break;

                default:
                    taken = damage;
                    break;
            }

            HP.Current -= taken;

            CombatEvent damaged = new CombatEventBuilder()
                .SetEventType(CombatEventType.DAMAGED)
                .AddField("amount", taken)
                .AddField("me", this)
                .Build();

            Logger.Report(this, $"Took {taken} damage. HP: {HP.Current} / {HP.Max}");

            GameManager.Instance.SendGameEvent(this, damaged);

            return taken;
        }

        /// <summary>
        /// Runs on this object's end phase
        /// Ticks down all effects currently affecting this object.
        /// </summary>
        public virtual void OnEndTurn()
        {

        }

        /// <summary>
        /// Builds a collection of control bases containing hud elements
        /// based off current object's stats. <br></br>
        /// Listen, the only way i could get this to update correctly was by rebuilding it every time an action is taken
        /// As a result keep this as efficient as possible.
        /// </summary>
        /// <returns>Collection of these control bases</returns>
        public virtual ICollection<ControlBase> GetHudElements()
        {
            List<ControlBase> list = new();

            ProgressBar hpBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = (float)HP.Current / (float)HP.Max,
                Position = new(5, 0),
                DisplayText = $"{HP.Current} / {HP.Max}"
            };
            hpBar.BarColor = Color.Red;
            list.Add(hpBar);

            Label label = new("HP:");
            label.Position = (0, 0);
            list.Add(label);

            return list;
        }

        public virtual void Blink()
        {
            AppearanceSurface.Animation.Start();
        }

        /// <summary>
        /// Required for deriving classes to invoke the OnAttack event
        /// </summary>
        /// <param name="event">Send event from weapons or spells here</param>
        protected void RaiseOnAttack(GameEvent @event)
        {
            OnAttack?.Invoke(this, @event);
        }

        /// <summary>
        /// Triggers DirectionChanged event
        /// </summary>
        /// <param name="old">old val</param>
        /// <param name="new">new val</param>
        protected void OnDirectionChanged(Data.Direction old, Data.Direction @new)
        {
            DirectionChanged?.Invoke(this, new(old, @new));
        }

        protected static void PlugMemoryLeaks(IEnumerable<ControlBase> controls)
        {
            foreach (ControlBase control in controls)
            {
                control.UseKeyboard = false;
            }
        }
    }
}
