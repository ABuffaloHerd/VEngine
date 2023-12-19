using SadConsole.Input;
using SadConsole.UI;
using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Objects;
using VEngine.Data;
using VEngine.Logging;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.UI.Controls;
using Newtonsoft.Json.Bson;
using System.Diagnostics;

namespace VEngine.Scenes.Combat
{
    public partial class CombatScene : Scene
    {
        private Console title;
        private Console bgm;
        private Console focus;
        private Console turnOrder;
        
        private ControlsConsole controls;
        private ControlsConsole hud;
        private ControlsConsole party;
        private FightFeed fightFeed;

        private Arena arena;

        private TurnQueue turn;

        private HashSet<GameObject> gameObjects;
        private HashSet<ControllableGameObject> players;

        private GameObject? selectedGameObject;

        private HashSet<Action<GameObject>> OnAttackEffects;

        public CombatScene() : base()
        {
            turn = new();

            SetupConsoles();
            SetupArena();
            SetupFightFeed();
            SetupControls();
            SetupTurnOrder();
            SetupFocus();
            SetupHud();
            SetupParty();

            gameObjects = new();
            players = new();
            selectedGameObject = null;

            /* ===== Test code ===== */
            AnimatedScreenObject animated = new("test", 1, 1);
            animated.CreateFrame()[0].Glyph = 'T';

            GameObject test = new(animated, 1);
            test.Name = "Testobj";

            GameObject test2 = new(animated, 1);
            test2.Name = "Test 2";
            test2.Speed = (Stat)200;

            GameObject test3 = new(animated, 1);
            test3.Name = "Test 3";
            test3.Speed = (Stat)90;

            GameObject test4 = new(animated, 1);
            test4.Name = "Test 4";
            test4.Speed = (Stat)190;

            GameObject test5 = new(animated, 1);
            test5.Name = "Test 5";
            test5.Speed = (Stat)20;

            AnimatedScreenObject aso = new("Controllable", 1, 1);
            aso.CreateFrame()[0].Glyph = (char)1;
            ControllableGameObject pgo = new(aso, 2);
            pgo.Name = "Controllable";
            pgo.Speed = 250;

            // put some walls
            AnimatedScreenObject wallobj = new("wall", 1, 1);
            wallobj.CreateFrame()[0].Glyph = (char)219;

            StaticGameObject wall = new(wallobj, 1);

            AnimatedScreenObject aso2 = new("Ranger", 1, 1);
            aso2.CreateFrame()[0].Glyph = 'M';
            aso2.Frames[0].SetForeground(0, 0, Color.Gray);
            Ranger ranger = new(aso2, 1);
            ranger.Name = "Minako";
            ranger.Speed = 120;


            AnimatedScreenObject aso3 = new("Mage", 1, 1);
            aso3.CreateFrame()[0].Glyph = 'S';
            aso3.Frames[0].SetForeground(0, 0, Color.PaleGoldenrod);
            Mage mage = new(aso3, 1);
            mage.Name = "Saki";
            mage.Speed = 90;

            //AddGameObject(test);
            //AddGameObject(test2);
            //AddGameObject(test3);
            //AddGameObject(test4);
            AddGameObject(test5);
            AddGameObject(pgo);
            AddGameObject(ranger);
            AddGameObject(wall);
            AddGameObject(mage);

            /* ===== End test code ===== */

            // Start the turn
            OnNextTurn();
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            arena.AddEntity(gameObject);

            turn.Enqueue(gameObject);

            turn.Sort();
        }

        public void RemoveGameObject(GameObject gameObject) 
        {
            gameObjects.Remove(gameObject);
            arena.RemoveEntity(gameObject);
            turn.Remove(gameObject);
            UpdateTurnConsole(); // re render the turn console to reflect these changes

            Logger.Report(this, $"Removed {gameObject}");
        }

        /// <summary>
        ///  Only run when an object is summoned.
        /// </summary>
        /// <param name="gameObject"></param>
        private void SummonGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            // special case for magic circles
            if(gameObject is MagicCircle)
            {
                arena.AddMagicCircle(gameObject as MagicCircle);
            }
        }

        /// <summary>
        /// Runs when it's a new object's turn
        /// </summary>
        private void OnNextTurn()
        {
            // Reset the arena
            arena.StopRenderPattern();

            // === This section handles turn ordering === //
            // If the turn order is empty, rebuild it.
            if (turn.Size <= 0)
            {
                Logger.Report(this, "Turnqueue rebuild triggered");
                turn.Rebuild(gameObjects);
            }

            turn.Sort();

            // === This section handles event subscriptions === //

            /**
             * The following events must be subscribed and unsubscribed from each turn:
             * - OnMove <-> PositionChanged
             * - OnAttack <-> OnAttack
             * - DirectionChanged <-> OnDirectionChanged
             */

            if (selectedGameObject != null)
            {
                selectedGameObject.PositionChanged -= OnMove;
                selectedGameObject.OnAttack -= OnAttack;
                selectedGameObject.DirectionChanged -= OnDirectionChanged;
            }
            selectedGameObject = turn.Dequeue();

            selectedGameObject.OnAttack += OnAttack;
            selectedGameObject.PositionChanged += OnMove;
            selectedGameObject.DirectionChanged += OnDirectionChanged;

            // === This section handles console setup === //
            UpdateHud();
            UpdateTurnConsole();

            // Copy in the new controls
            controls.Controls.Clear();
            if (selectedGameObject is IControllable)
            {
                IControllable? controllable = selectedGameObject as IControllable;

                foreach (ControlBase conhost in controllable.GetControls())
                {
                    controls.Controls.Add(conhost);
                }

                controls.IsEnabled = true;
            }
            else
            {
                controls.IsEnabled = false;
            }

            // Reset the current object's move value
            selectedGameObject.MoveDist.ResetCurrent = true;

            // Alert the fight feed
            fightFeed.Print($"It is now {selectedGameObject.Name}'s turn.");
        }

        /// <summary>
        /// Turns the turn order into a console representation
        /// </summary>
        private void UpdateTurnConsole()
        {
            turnOrder.Clear();

            // The first slot is reserved for the current object
            turnOrder.Print(0, 0, "[c:r b:yellow][c:r f:black]" + selectedGameObject.Name);

            // then for every slot below put the other objects
            int y = 0;
            foreach (var obj in turn)
            {
                turnOrder.Print(0, ++y, obj.Name);
            }
        }

        private void UpdateHud()
        {
            hud.Controls.Clear();
            hud.Surface.Clear(0, 17, 20);
            //hud.Print(0, 0, selectedGameObject.Name);
            hud.Print(0, 17, $"M: {selectedGameObject.MoveDist}");

            // get controls from the current game object
            ICollection<ControlBase> controlBases = selectedGameObject.GetHudElements();
            foreach(ControlBase control in controlBases)
            {
                hud.Controls.Add(control);
            }

        }


        /// <summary>
        /// Runs when the current game object has it's position changed.
        /// </summary>
        private void OnMove(object? sender, ValueChangedEventArgs<Point> args) 
        {
            // If the arena was rendering a pattern rerender it
            if(arena.IsRenderingPattern)
                arena.RenderCachedPattern(args.NewValue, selectedGameObject.Facing);

            // tell the arena to update all its positions
            arena.UpdatePositions(args);
        }

        /// <summary>
        /// Runs when an attack is executed. Reports information to fight feed. <br></br>
        /// The event received must have the following fields: <br></br>
        ///     - targets : list of targets that were attacked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">game event</param>
        private void OnAttack(object? sender, GameEvent args)
        {
            // Get the targets out of the game event
            IEnumerable<GameObject> targets = args.GetData<IEnumerable<GameObject>>("targets");

            foreach(GameObject target in targets)
            {
                Logger.Report(sender, $"Attacked {target}. {target}'s HP: {target.HP.Current} / {target.HP.Max}");

                // check if the target is dead
                if(target.IsDead)
                {
                    Logger.Report(this, $"{target} died.");
                    fightFeed.Print($"{target} died.");
                    RemoveGameObject(target);
                }
            }
        }

        /// <summary>
        /// Runs when an object turns its head
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDirectionChanged(object? sender, ValueChangedEventArgs<Data.Direction> args)
        {
            // all this does is re render any patterns
            if (arena.IsRenderingPattern)
                arena.RenderCachedPattern(selectedGameObject.Position, args.NewValue);
        }

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
                    switch(kpe.SadKey)
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
            switch(e.EventType)
            {
                case CombatEventType.DAMAGED:
                    Logger.Report(this, e.ToString());
                    int damage = e.GetData<int>("amount");
                    string thing = e.GetData<GameObject>("me").Name;

                    fightFeed.Print($"{thing} took {damage} damage");
                    break;

                case CombatEventType.ACTION:
                    // figure out which action to do etc
                    // debug code below is to attack one tile ahead
                    Pattern p2 = new();
                    p2.Mark(0, 0);
                    p2.Mark(1, 0);
                    ExecuteAttack(selectedGameObject, p2);

                    break;

                case CombatEventType.SUMMON:
                    // figure out what to summon
                    switch(e.GetData<string>("summon"))
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

                            foreach(Point p in l)
                            {
                                if(arena.IsTileFree(selectedGameObject.Position + p, true))
                                {
                                    MagicCircle mc = new(Color.Magenta, Alignment.FRIEND)
                                    {
                                        Position = selectedGameObject.Position + p
                                    };

                                    SummonGameObject(mc);

                                    fightFeed.Print("Summoned magic circle!");
                                    break;
                                }    
                            }

                            break;
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
            if(e is KeyPressedEvent)
            {
                ProcessKeyEvent(e as KeyPressedEvent);
            }

            if(e is CombatEvent)
            {
                ProcessCombatEvent(e as CombatEvent);
            }

            UpdateHud();
        }
    }
}
