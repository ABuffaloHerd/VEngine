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
using VEngine.Animations;

using Microsoft.Xna.Framework.Graphics;
using SadConsole.UI.Controls;

using Effect = VEngine.Effects.Effect;
using VEngine.Factory;
using VEngine.Effects;

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

        private HashSet<ArenaEffect> OnCycleEffects;
        private HashSet<EntityEffect> OnAttackEffects;
        private HashSet<EntityEffect> OnTurnStartEffects;

        private static int instanceCount = 0;
        private CombatScene() : base()
        {
            Logger.Report(this, $"CombatScene being created: {this.GetHashCode()} ");
            if (instanceCount >= 1 ) throw new Exception("Somebody is creating two combat scenes.");
            instanceCount++;
            turn = new();

            SetupConsoles();
            SetupFightFeed();
            SetupControls();
            SetupTurnOrder();
            SetupFocus();
            SetupHud();
            SetupParty();

            gameObjects = new();
            players = new();
            selectedGameObject = null;

            OnAttackEffects = new();
            OnCycleEffects = new();
            OnTurnStartEffects = new();

            // Test on turn conditions
            EntityEffect effect = new("Test", "Take 1 true damage at start of turn", 1,
                (obj) =>
                {
                    obj.TakeDamage(null, null, 1, DamageType.TRUE);

                    CombatEvent ce = new CombatEventBuilder()
                        .SetEventType(CombatEventType.INFO)
                        .AddField("content", $"Dealt 1 damage to {obj}")
                        .Build();

                    return ce;
                }
            );
            //OnTurnStartEffects.Add(effect);

            Focused += (s, a) =>
            {
                Logger.Report(this, "yeah focused");
            };

            FocusLost += (s, a) =>
            {
                Logger.Report(this, "focus lost");
                Logger.Report(this, $"Game is focused on {Game.Instance.FocusedScreenObjects.ToString()}");
                IsFocused = true;
            };
        }

        public CombatScene(CombatScenario preset) : this()
        {
            SetupArena(preset.ArenaWidth, preset.ArenaHeight);

            foreach(var obj in preset.Objects)
                AddGameObject(obj);

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

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            // check if this is a controllable to begin with
            if(selectedGameObject is AIControlledGameObject)
            {
                Logger.Report(this, "AI controlled object in play");
                // pass control to AI handler function.

                HandleAI(selectedGameObject as AIControlledGameObject);
                return false;
            }    

            if(keyboard.HasKeysDown) 
            {
                foreach(AsciiKey k in keyboard.KeysPressed) 
                {
                    switch (k.Key)
                    {
                        case Keys.W:
                            Move(new Point(0, -1));
                            break;

                        case Keys.A:
                            Move(new Point(-1, 0));
                            break;

                        case Keys.S:
                            Move(new Point(0, 1));
                            break;

                        case Keys.D:
                            Move(new Point(1, 0));
                            break;

                        case Keys.Space:
                            OnNextTurn();
                            break;

                        /// === LOOKING KEYS === ///
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


                        /// === TESTING KEYS === ///
                        case Keys.J:
                            Logger.Report(this, "j pressed");

                            if (arena.IsRenderingPattern)
                            {
                                arena.StopRenderPattern();
                            }
                            else
                            {
                                // feature not a bug
                                if (selectedGameObject is not ControllableGameObject) break;
                                else
                                {
                                    Pattern p = (selectedGameObject as ControllableGameObject).GetRange();
                                    arena.RenderPattern(p, selectedGameObject.Position, selectedGameObject.Facing);
                                }
                            }

                            break;

                        case Keys.L:
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
            }

            return true;
        }

        public override string ToString()
        {
            return "CombatScene";
        }

        /// <summary>
        ///  Only run when an object is summoned.
        /// </summary>
        /// <param name="gameObject"></param>
        private void SummonGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            // special case for magic circles

            if (gameObject is MagicCircle)
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
                Logger.Report(this, "Next cycle has started");
                fightFeed.Print("The next cycle has begun.");
                fightFeed.Print("");
                turn.Rebuild(gameObjects);

                // Trigger on next cycle function
                OnNextCycle();
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

            // Trigger select game object's on turn start function
            selectedGameObject.OnStartTurn();

            // Trigger the scene's global on turn start 
            foreach(var effect in OnTurnStartEffects)
            {
                effect.ApplyEffect(selectedGameObject);
            }

            CheckAllIfDead();
        }

        private void OnNextCycle()
        {
            foreach(var effect in OnCycleEffects)
            {
                effect.ApplyEffect(gameObjects);

                if(!effect.IsInfinite && effect.Timer <= 0)
                {
                    OnCycleEffects.Remove(effect);
                    fightFeed.Print($"Effect {effect.Name} has expired.");
                }
            }

            CheckAllIfDead();
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

            hud.Print(0, 17, $"m: {selectedGameObject.MoveDist}");

            // get controls from the current game object
            ICollection<ControlBase> controlBases = selectedGameObject.GetHudElements();
            foreach (ControlBase control in controlBases)
            {
                hud.Controls.Add(control);
            }
        }


        /// <summary>
        /// Runs when the current game object has it's position changed.
        /// </summary>
        private void OnMove(object? sender, ValueChangedEventArgs<Point> args)
        {
            // tell the arena to update all its positions
            arena.UpdatePositions(args);

            // If the arena was rendering a pattern rerender it
            if (arena.IsRenderingPattern)
                arena.RenderCachedPattern(args.NewValue, selectedGameObject.Facing);

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

            foreach (GameObject target in targets)
            {
                Logger.Report(sender, $"Attacked {target}. {target}'s HP: {target.HP.Current} / {target.HP.Max}");

                CheckIfDead(target);
            }
        }

        /// <summary>
        /// Alright who's not dead sound off.
        /// </summary>
        /// <param name="target"></param>
        private void CheckIfDead(GameObject target)
        {
            if (target.IsDead)
            {
                fightFeed.Print($"{target} died.");
                RemoveGameObject(target);
            }
        }

        /// <summary>
        /// 💀
        /// </summary>
        private void CheckAllIfDead()
        {
            foreach (GameObject obj in gameObjects)
                CheckIfDead(obj);
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
    }
}
