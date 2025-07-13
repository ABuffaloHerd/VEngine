using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using VEngine.Data;
using VEngine.Events;
using VEngine.Factory;
using VEngine.Items;
using VEngine.Logging;
using VEngine.Scenes.Combat;

namespace VEngine.Objects.Classes
{
    // Watch out sniper
    public class Ranger : ControllableGameObject
    {
        public Stat Ammo { get; set; } = 2;
        public Ranger(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            Weapon = WeaponRegistry.Rifle.Clone() as Weapon;
        }

        public override ICollection<ControlBase> GetHudElements()
        {
            ICollection<ControlBase> list = base.GetHudElements();

            ProgressBar ammobar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = Ammo.Current / (float)Ammo.Max,
                Position = (6, 10),
                DisplayText = $"{Ammo.Current} / {Ammo.Max}",
                BarColor = Color.Brown,
                DisplayTextColor = Color.Lavender
            };
            Label ammolabel = new("AMMO: ");
            ammolabel.Position = (0, 10);

            list.Add(ammobar);
            list.Add(ammolabel);

            return list;
        }

        public override void Attack(IEnumerable<GameObject> targets, Arena arena)
        {
            if (Ammo.Current - 1 < 0)
            {
                Logger.Report(this, "out of ammo!");
                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.INFO)
                    .AddField("content", $"{Name}: Out of ammo!")
                    .Build();

                GameManager.Instance.SendGameEvent(this, ev);

                return;
            }
            
            // Decrement ammo BEFORE triggering the attack event
            // This ensures the HUD shows the correct ammo count when updated
            Ammo--;
            
            base.Attack(targets, arena);
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
    }
}
