using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Logging;
using VEngine.Objects;
using VEngine.Data;
using SadConsole.Input;
using VEngine.Items;
using VEngine.Objects.Classes;

namespace VEngine.Scenes.Combat
{
    public partial class CombatScene
    {
        private void ProcessKeyEvent(KeyPressedEvent kpe)
        {
            switch (kpe.Key)
            {
                case 'w':
                    Move(new Point(0, -1));
                    break;

                case 'a':
                    Move(new Point(-1, 0));
                    break;

                case 's':
                    Move(new Point(0, 1));
                    break;

                case 'd':
                    Move(new Point(1, 0));
                    break;

                case ' ':
                    OnNextTurn();
                    break;

                /// === LOOKING KEYS === ///
                case (char)37:
                case (char)38:
                case (char)39:
                case (char)40:
                    switch (kpe.SadKey)
                    {
                        case Keys.Up:
                            selectedGameObject.Facing = Data.Direction.UP;
                            break;
                        case Keys.Down:
                            selectedGameObject.Facing = Data.Direction.DOWN;
                            break;
                        case Keys.Left:
                            selectedGameObject.Facing = Data.Direction.LEFT;
                            break;
                        case Keys.Right:
                            selectedGameObject.Facing = Data.Direction.RIGHT;
                            break;
                    }
                    break;


                /// === TESTING KEYS === ///
                case 'j':
                    Logger.Report(this, "j pressed");

                    if (arena.IsRenderingPattern)
                    {
                        arena.StopRenderPattern();
                    }
                    else
                    {
                        // feature not a bug
                        if (selectedGameObject is not ControllableGameObject) return;
                        else
                        {
                            Pattern p = (selectedGameObject as ControllableGameObject).GetRange();
                            arena.RenderPattern(p, selectedGameObject.Position, selectedGameObject.Facing);
                        }
                    }

                    break;

                case 'l':
                    Pattern p2 = new();
                    p2.Mark(0, 0);
                    p2.Mark(1, 0);
                    if (selectedGameObject is ControllableGameObject)
                    {
                        ExecuteAttack(selectedGameObject, (selectedGameObject as ControllableGameObject).GetRange());
                    }
                    else
                        ExecuteAttack(selectedGameObject, p2);

                    break;
            }
        }

        private void ProcessCombatEvent(CombatEvent e)
        {
            // check the type of combat event before processing.
            switch (e.EventType)
            {
                case CombatEventType.DAMAGED:
                    if (e.TryGetData<int>("amount", out var damage) && e.TryGetData<GameObject>("me", out var damagedObject))
                    {
                        fightFeed.Print($"{damagedObject.Name} took {damage} damage");
                    }
                    break;

                case CombatEventType.ACTION:
                    // figure out which action to do etc
                    if (e.TryGetData<string>("action", out var what))
                    {
                        switch (what)
                        {
                            case "show_range":
                                // get range and tell arena to display it
                                if (e.TryGetData<Pattern>("pattern", out var pattern))
                                {
                                    arena.RenderPattern(pattern, selectedGameObject.Position, selectedGameObject.Facing);
                                }
                                break;

                            case "attack":
                                // run attack logic
                                ExecuteAttack(selectedGameObject, selectedGameObject.Range);
                                break;
                        }
                    }
                    break;

                case CombatEventType.INFO:
                    // print to fight feed
                    if (e.TryGetData<string>("content", out var data))
                    {
                        fightFeed.Print(data);
                    }
                    break;

                case CombatEventType.SUMMON:
                    // figure out what to summon
                    if (e.TryGetData<string>("summon", out var summonType))
                    {
                        switch (summonType)
                        {
                            case "magic_circle":
                                // Check if we have position and owner data
                                if (e.TryGetData<Point>("position", out var position) && 
                                    e.TryGetData<Mage>("owner", out var owner))
                                {
                                    // Create magic circle at specified position
                                    MagicCircle mc = new(Color.Magenta, Alignment.FRIEND, owner)
                                    {
                                        Position = position
                                    };

                                    SummonGameObject(mc);
                                    fightFeed.Print($"{owner.Name} summoned a magic circle!");
                                }
                                else
                                {
                                    // Fallback to old behavior for compatibility
                                    // check 4 adjacent tiles to see if they're free
                                    List<Point> l = new()
                                    {
                                        (1, 0),
                                        (0, 1),
                                        (-1, 0),
                                        (0, -1)
                                    };

                                    foreach (Point p in l)
                                    {
                                        if (arena.IsTileFree(selectedGameObject.Position + p, true))
                                        {
                                            MagicCircle mc = new(Color.Magenta, Alignment.FRIEND, selectedGameObject)
                                            {
                                                Position = selectedGameObject.Position + p
                                            };

                                            SummonGameObject(mc);
                                            fightFeed.Print("Summoned magic circle!");

                                            // Update the mage's circle count if it's a mage
                                            if (selectedGameObject is Mage mage)
                                            {
                                                mage.MagicCircles = arena.CountMagicCircle();
                                            }

                                            break;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case CombatEventType.CAST:
                    // Get the spell and send it
                    if (e.TryGetData<Spell>("spell", out var spell))
                    {
                        CastSpell(selectedGameObject, spell.Range, spell);
                    }
                    break;
            }
        }

        /// <summary>
        /// All events come to this function before being dispatched to handler functions
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">The event</param>
        protected override void ProcessGameEvent(object sender, IGameEvent e)
        {
            if (e is KeyPressedEvent keyEvent)
            {
                ProcessKeyEvent(keyEvent);
            }

            if (e is CombatEvent combatEvent)
            {
                ProcessCombatEvent(combatEvent);
            }

            // Only update HUD for events that actually change game state
            if (e is KeyPressedEvent || e is CombatEvent)
            {
                UpdateHud();
            }
        }
    }
}
