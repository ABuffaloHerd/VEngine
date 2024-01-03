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
                    int damage = e.GetData<int>("amount");
                    string thing = e.GetData<GameObject>("me").Name;

                    fightFeed.Print($"{thing} took {damage} damage");
                    break;

                case CombatEventType.ACTION:
                    // figure out which action to do etc
                    string what = e.GetData<string>("action");
                    switch (what)
                    {
                        case "show_range":
                            // get range and tell arena to display it
                            Pattern p = e.GetData<Pattern>("pattern");
                            arena.RenderPattern(p, selectedGameObject.Position, selectedGameObject.Facing);
                            break;
                    }

                    break;

                case CombatEventType.INFO:
                    // print to fight feed
                    string data = e.GetData<string>("content");

                    fightFeed.Print(data);
                    break;

                case CombatEventType.SUMMON:
                    // figure out what to summon
                    switch (e.GetData<string>("summon"))
                    {
                        case "magic_circle":
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

                                    // Since only mages can summon magic circles, it is safe to assume that if we made it this far, the selected game object is a mage.
                                    (selectedGameObject as Mage).MagicCircles = arena.CountMagicCircle();

                                    break;
                                }
                            }

                            break;
                    }

                    break;

                case CombatEventType.CAST:
                    // Get the spell and send it
                    Spell castme = e.GetData<Spell>("spell");
                    CastSpell(selectedGameObject, castme.Range, castme);

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
            if (e is KeyPressedEvent)
            {
                ProcessKeyEvent(e as KeyPressedEvent);
            }

            if (e is CombatEvent)
            {
                ProcessCombatEvent(e as CombatEvent);
            }

            UpdateHud();
        }
    }
}
