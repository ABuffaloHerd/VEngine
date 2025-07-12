using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using VEngine.Components;
using VEngine.Data;
using VEngine.Events;
using VEngine.Factory;
using VEngine.Items;
using VEngine.Logging;
using VEngine.Scenes.Combat;

namespace VEngine.Objects
{
    public class ControllableGameObject : GameObject, IControllable
    {
        protected Weapon Weapon
        {
            get
            {
                return GetComponent<WeaponComponent>().Weapon;
            }
            set
            {
                GetComponent<WeaponComponent>().Weapon = value;
            }
        }

        public Stat MP { get; set; }
        public Stat SP { get; set; }

        public override Pattern Range
        {
            get
            {
                return Weapon.Range;
            }
        }

        public ControllableGameObject(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            // default for all controllables
            Alignment = Alignment.FRIEND;

            MP = 10;
            SP = 5;

            // set attributes for MP bar
            MP.IsOverloadable = true;

            AddComponent(new CollisionComponent());
            AddComponent(new WeaponComponent());
            //hudElements = GetHudElements();
        }

        public Pattern GetRange()
        {
            return GetComponent<WeaponComponent>().Range;
        }

        // todo: maybe change this so that controls aren't instatiated every fucking time?
        public virtual ICollection<ControlBase> GetControls()
        {
            return new List<ControlBase>();
        }

        public override ICollection<ControlBase> GetHudElements()
        {
            var list = base.GetHudElements();

            ProgressBar mpBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = MP.Current / (float)MP.Max,
                Position = (5, 2),
                DisplayText = (MP.Overloaded ? "Overload! " : "") + $"{MP.Current} / {MP.Max}",
                BarColor = MP.Overloaded ? Color.Purple : Color.Blue,
            };
            Label mplabel = new("MP: ")
            {
                Position = (0, 2)
            };
            list.Add(mpBar);
            list.Add(mplabel);

            ProgressBar spBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = SP.Current / (float)SP.Max,
                Position = (5, 4),
                DisplayText = $"{SP.Current} / {SP.Max}",
                DisplayTextColor = Color.Black,
                BarColor = Color.Yellow,
                BackgroundGlyph = 178
            };
            Label spLabel = new("SP: ")
            {
                Position = (0, 4)
            };
            list.Add(spBar);
            list.Add(spLabel);

            if(HasComponent<OverdriveComponent>())
            {
                OverdriveComponent odComponent = GetComponent<OverdriveComponent>();
                ProgressBar odBar = new(20, 1, HorizontalAlignment.Left)
                {
                    Progress = odComponent.Stat.Current / (float)100,
                    Position = (5, 8),
                    DisplayText = $"{odComponent.Stat.Current} / 100",
                    DisplayTextColor = Color.Black,
                    BarColor = Color.DarkOrange,
                    BackgroundGlyph = 178,
                };
                Label odLabel = new("OD: ")
                {
                    Position = (0, 8)
                };
                list.Add(odLabel);
                list.Add(odBar);
            }


            PlugMemoryLeaks(list);
            return list;
        }

        public override void Attack(IEnumerable<GameObject> targets, Arena arena)
        {
            // instead of doing fuck all like the base class, use the weapon object's attack function
            var ev = Weapon.ApplyEffect(targets, this, arena);

            // always trigger OnAttack
            RaiseOnAttack(ev);
        }

        /// <summary>
        /// Attack method using arena context
        /// </summary>
        public void Attack(IEnumerable<GameObject> targets)
        {
            if (HasArena)
            {
                Attack(targets, Arena!);
            }
            else
            {
                Logger.Report(this, "No arena available for attack");
            }
        }

        public override void Cast(IEnumerable<GameObject> targets, Arena arena, Spell spell)
        {
            if (MP.Current - spell.Cost < 0)
            {
                // Send failure event to fight feed
                var failureEvent = new CombatEventBuilder()
                    .SetEventType(CombatEventType.INFO)
                    .AddField("content", $"{Name} doesn't have enough MP to cast {spell.Name}!")
                    .Build();
                GameManager.Instance.SendGameEvent(this, failureEvent);
                return;
            }

            MP -= spell.Cost;
            Logger.Report(this, $"{Name} cast {spell.Name} for {spell.Cost} MP");
            Logger.Report(this, $"{Name} has {MP.Current} MP remaining");

            var ev = spell.ApplyEffect(targets, this, arena);
            RaiseOnAttack(ev);
        }

        /// <summary>
        /// Cast method using arena context
        /// </summary>
        public void Cast(IEnumerable<GameObject> targets, Spell spell)
        {
            if (HasArena)
            {
                Cast(targets, Arena!, spell);
            }
            else
            {
                Logger.Report(this, "No arena available for casting");
            }
        }

        public override int TakeDamage(GameObject? attacker, ICombatItem? item, int damage, DamageType type)
        {
            int incoming = base.TakeDamage(attacker, item, damage, type);
            // on top of this, turn 20% of the damage taken into OD charge, if the class supports it.
            if (HasComponent<OverdriveComponent>())
            {
                int charge = (int)Math.Ceiling(0.2 * incoming);
                GetComponent<OverdriveComponent>().Stat += charge;
            }
            return incoming;
        }
    }
}
