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

            OnAttackEffects = new();
            OnCycleEffects = new();
            OnTurnStartEffects = new();

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
            wall.Position = (3, 3);

            StaticGameObject wall2 = new(wallobj, 1)
            {
                Position = (3, 2)
            };

            StaticGameObject wall3 = new(wallobj, 1)
            {
                Position = (3, 4)
            };

            //AnimatedScreenObject aso2 = new("Ranger", 1, 1);
            //aso2.CreateFrame()[0].Glyph = 'M';
            //aso2.Frames[0].SetForeground(0, 0, Color.Gray);
            AnimatedScreenObject aso2 = AnimationPresets.BlinkingEffect("Ranger", 'M', Color.Gray, Color.Black, (1, 1));
            Ranger ranger = new(aso2, 1);
            ranger.Name = "Minako";
            ranger.Speed = 120;
            ranger.Position = (7, 6);


            //AnimatedScreenObject aso3 = new("Mage", 1, 1);
            //aso3.CreateFrame()[0].Glyph = 'S';
            //aso3.Frames[0].SetForeground(0, 0, Color.PaleGoldenrod);
            AnimatedScreenObject aso3 = AnimationPresets.BlinkingEffect("Mage", 'S', Color.PaleGoldenrod, Color.Black, (1, 1));
            Mage mage = new(aso3, 1);
            mage.Name = "Saki";
            mage.Speed = 90;
            mage.Position = (0, 3);

            AnimatedScreenObject aso4 = AnimationPresets.BlinkingEffect("Mage2", 'E', Color.LightGray, Color.Black, (1, 1));
            Mage mage2 = new(aso4, 1);
            mage2.Name = "Elaine";
            mage2.Speed = 101;
            mage2.Position = (0, 5);
            mage2.RES = 50;
            mage2.DEF = 2;

            AnimatedScreenObject aso5 = AnimationPresets.BlinkingEffect("Guard", 'H', Color.Red, Color.Black, (1, 1));
            Guard guard = new(aso5, 1);
            guard.Name = "Hirina";
            guard.Speed = 100;
            guard.Position = (1, 3);

            //AddGameObject(test);
            //AddGameObject(test2);
            //AddGameObject(test3);
            //AddGameObject(test4);
            //AddGameObject(test5);
            //AddGameObject(pgo);
            AddGameObject(ranger);
            AddGameObject(wall);
            AddGameObject(wall2);
            AddGameObject(wall3);
            AddGameObject(mage);
            AddGameObject(mage2);
            AddGameObject(guard);

            // Test on turn conditions
            EntityEffect effect = new("Test", "Take 1 true damage at start of turn", 1,
                (obj) =>
                {
                    obj.TakeDamage(1, DamageType.TRUE);

                    CombatEvent ce = new CombatEventBuilder()
                        .SetEventType(CombatEventType.INFO)
                        .AddField("content", $"Dealt 1 damage to {obj}")
                        .Build();

                    return ce;
                }
            );
            //OnTurnStartEffects.Add(effect);

            // Test on cycle conditions
            ArenaEffect arenaEffect = new("test 2", "take 2 true damage per cycle", 1,
                (objs) =>
                {
                    foreach(var obj in objs)
                    {
                        obj.TakeDamage(2, DamageType.TRUE);
                    }

                    return null;
                });
            OnCycleEffects.Add(arenaEffect);

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

            //hud.Print(0, 17, $"M: {selectedGameObject.MoveDist}");

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
