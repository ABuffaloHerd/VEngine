using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Factory;
using VEngine.Items;

namespace VEngine.Objects
{
    /// <summary>
    /// Mages get a mana boost for every magic circle owned by them
    /// </summary>
    public class Mage : ControllableGameObject
    {
        public int MagicCircles { get; set; }
        private List<Spell> spells;
        public Mage(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            MagicCircles = 0;
            // testing 
            spells = new()
            {
                (Spell)SpellRegistry.Fireball.Clone()
            };
        }

        public override void OnStartTurn()
        {
            base.OnStartTurn(); // must call this because it handles effects

            // Passive: Mages gain 10 max MP for each magic circle and regenerate 5 MP
            MP.Max = 10 + (MagicCircles * 10);
            for (int x = 0; x < MagicCircles; x++)
            {
                MP.Current += 5;
            }
        }

        public override ICollection<ControlBase> GetControls()
        {
            Button b = new("Summon")
            {
                Position = (0, 0)
            };
            b.Click += (s, e) =>
            {
                // summon a magic circle
                CombatEvent combat = new CombatEventBuilder()
                    .SetEventType(CombatEventType.SUMMON)
                    .AddField("summon", "magic_circle")
                    .Build();

                if ((MP - 10) < 0) return;
                this.MP -= 10; // costs 10 mana

                GameManager.Instance.SendGameEvent(this, combat);
            };

            List<ControlBase> controls = new()
            {
                b
            };

            int y = 2;
            foreach(Spell spell in spells)
            {
                Button b2 = new(spell.Name);
                b2.Position = (0, ++y);
                b2.Click += (s, e) =>
                {
                    CombatEvent ce = new CombatEventBuilder()
                        .SetEventType(CombatEventType.CAST)
                        .AddField("spell", spell)
                        .Build();

                    GameManager.Instance.SendGameEvent(this, ce);
                };

                Button b3 = new("range")
                {
                    // 20 looks like a magic number because it is. 27 is the size of the controls console
                    // minus (5, length of "range" + 2 for button padding)
                    Position = (20, y)
                };
                b3.Click += (s, e) =>
                {
                    CombatEvent ce = new CombatEventBuilder()
                        .SetEventType(CombatEventType.ACTION)
                        .AddField("action", "show_range")
                        .AddField("pattern", spell.Range)
                        .Build();

                    GameManager.Instance.SendGameEvent(this, ce);
                };

                controls.Add(b2);
                controls.Add(b3);
            }

            return controls;
        }
    }
}
