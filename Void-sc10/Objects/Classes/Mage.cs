using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Factory;
using VEngine.Items;

namespace VEngine.Objects.Classes
{
    /// <summary>
    /// Mages get a mana boost for every magic circle owned by them
    /// </summary>
    /// Fireball!
    public class Mage : ControllableGameObject
    {
        public int MagicCircles { get; set; }
        private List<Spell> spellBook;
        private Spell selectedSpell;
        public Mage(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            MagicCircles = 0;
            selectedSpell = null;
            // testing 
            spellBook = new()
            {
                (Spell)SpellRegistry.Fireball.Clone(),
                (Spell)SpellRegistry.Lightning.Clone(),
                (Spell)SpellRegistry.ArcaneBlast.Clone()
            };
        }

        public override void OnStartTurn()
        {
            base.OnStartTurn(); // must call this because it handles effects

            // Passive: Mages gain 10 max MP for each magic circle and regenerate 5 MP
            MP.Max = MP.Max + MagicCircles * 10;
            for (int x = 0; x < MagicCircles; x++)
            {
                MP.Current += 5;
            }
        }

        public override ICollection<ControlBase> GetControls()
        {
            Button b = new("Summon magic circle")
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

                if (MP - 10 < 0) return;
                MP -= 10; // costs 10 mana

                GameManager.Instance.SendGameEvent(this, combat);
            };

            Label title = new("[c:r b:yellow][c:r f:black]Spellbook")
            {
                Position = (0, 2)
            };
            title.Surface.UsePrintProcessor = true;

            ListBox spellList = new(27, 5)
            {
                Position = (0, 4),
            };
            spellList.SelectedItemChanged += (s, args) =>
            {
                selectedSpell = (Spell)args.Item;
            };

            foreach (Spell spell in spellBook)
            {
                spellList.Items.Add(spell);
            }
            if (spellBook.Count > 0)
                spellList.SelectedIndex = 0;

            Button castButton = new("Cast")
            {
                Position = (0, 15)
            };
            castButton.Click += (s, e) =>
            {
                if (selectedSpell == null) return;
                CombatEvent ce = new CombatEventBuilder()
                    .SetEventType(CombatEventType.CAST)
                    .AddField("spell", selectedSpell)
                    .Build();

                GameManager.Instance.SendGameEvent(this, ce);
            };

            Button showRange = new("Range")
            {
                Position = (9, 15)
            };
            showRange.Click += (s, e) =>
            {
                if (selectedSpell == null) return;
                CombatEvent ce = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ACTION)
                    .AddField("action", "show_range")
                    .AddField("pattern", selectedSpell.Range)
                    .Build();

                GameManager.Instance.SendGameEvent(this, ce);
            };

            List<ControlBase> controls = new()
            {
                b,
                title,
                spellList,
                castButton,
                showRange
            };

            PlugMemoryLeaks(controls);
            return controls;
        }
    }
}
