using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Factory;
using VEngine.Items;
using VEngine.Scenes.Combat;

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
        private List<MagicCircle> ownedCircles = new();
        
        // Summoning configuration
        private const int SUMMON_COST = 10;
        private const int MAX_CIRCLES = 5; // Prevent infinite summoning
        private const int MP_PER_CIRCLE = 10;
        private const int MP_REGEN_PER_CIRCLE = 5;
        
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
            MP.Max = MP.Max + MagicCircles * MP_PER_CIRCLE;
            for (int x = 0; x < MagicCircles; x++)
            {
                MP.Current += MP_REGEN_PER_CIRCLE;
            }
        }

        /// <summary>
        /// Get valid summon positions around the mage
        /// </summary>
        private List<Point> GetValidSummonPositions(Arena arena)
        {
            var validPositions = new List<Point>();
            var adjacentPositions = new List<Point>
            {
                Position + new Point(1, 0),   // Right
                Position + new Point(0, 1),   // Down
                Position + new Point(-1, 0),  // Left
                Position + new Point(0, -1),  // Up
                Position + new Point(1, 1),   // Diagonal: Right-Down
                Position + new Point(-1, 1),  // Diagonal: Left-Down
                Position + new Point(1, -1),  // Diagonal: Right-Up
                Position + new Point(-1, -1)  // Diagonal: Left-Up
            };

            foreach (var pos in adjacentPositions)
            {
                if (arena.IsTileFree(pos, true) && arena.IsWithinBounds(pos))
                {
                    validPositions.Add(pos);
                }
            }

            return validPositions;
        }

        /// <summary>
        /// Find the best summon position based on tactical considerations
        /// </summary>
        private Point? FindBestSummonPosition(Arena arena)
        {
            var validPositions = GetValidSummonPositions(arena);
            
            if (validPositions.Count == 0)
                return null;

            // Priority: Adjacent positions first, then diagonals
            var adjacent = validPositions.Where(p => 
                Math.Abs(p.X - Position.X) + Math.Abs(p.Y - Position.Y) == 1).ToList();
            
            if (adjacent.Count > 0)
                return adjacent[0]; // Return first adjacent position
            
            return validPositions[0]; // Return first valid position
        }

        /// <summary>
        /// Attempt to summon a magic circle
        /// </summary>
        public bool TrySummonMagicCircle(Arena arena)
        {
            // Check prerequisites
            if (MP.Current < SUMMON_COST)
            {
                Logger.Report(this, "Not enough MP to summon magic circle");
                return false;
            }

            if (MagicCircles >= MAX_CIRCLES)
            {
                Logger.Report(this, "Maximum number of magic circles reached");
                return false;
            }

            // Find best position
            var summonPosition = FindBestSummonPosition(arena);
            if (summonPosition == null)
            {
                Logger.Report(this, "No valid position to summon magic circle");
                return false;
            }

            // Deduct MP and create circle
            MP.Current -= SUMMON_COST;
            
            var magicCircle = new MagicCircle(Color.Magenta, Alignment.FRIEND, this)
            {
                Position = summonPosition.Value
            };

            // Add to owned circles and update count
            ownedCircles.Add(magicCircle);
            MagicCircles = ownedCircles.Count;

            // Send summon event
            var combatEvent = new CombatEventBuilder()
                .SetEventType(CombatEventType.SUMMON)
                .AddField("summon", "magic_circle")
                .AddField("position", summonPosition.Value)
                .AddField("owner", this)
                .Build();

            GameManager.Instance.SendGameEvent(this, combatEvent);
            
            Logger.Report(this, $"Successfully summoned magic circle at {summonPosition.Value}");
            return true;
        }

        /// <summary>
        /// Remove a magic circle when it's destroyed
        /// </summary>
        public void RemoveMagicCircle(MagicCircle circle)
        {
            if (ownedCircles.Remove(circle))
            {
                MagicCircles = ownedCircles.Count;
                Logger.Report(this, $"Magic circle removed. Total circles: {MagicCircles}");
            }
        }

        public override ICollection<ControlBase> GetControls()
        {
            Button summonButton = new($"Summon Circle ({SUMMON_COST} MP)")
            {
                Position = (0, 0)
            };
            summonButton.Click += (s, e) =>
            {
                // Get arena reference (this is a bit hacky, but works for now)
                var combatScene = GameManager.Instance.GetType().GetField("currentScene", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(GameManager.Instance) as CombatScene;
                
                if (combatScene != null)
                {
                    var arena = combatScene.GetType().GetField("arena", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(combatScene) as Arena;
                    
                    if (arena != null)
                    {
                        TrySummonMagicCircle(arena);
                    }
                }
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
                summonButton,
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
