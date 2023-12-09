﻿using SadConsole.Input;
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

        private GameObject selectedGameObject;

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

            AddGameObject(test);
            AddGameObject(test2);
            AddGameObject(test3);
            AddGameObject(test4);
            AddGameObject(test5);
            AddGameObject(pgo);

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

        /// <summary>
        /// Runs when it's a new object's turn
        /// </summary>
        private void OnNextTurn()
        {
            // Reset the arena
            arena.ClearEffects();

            turn.Sort();

            /* 
             * The Pop phase selects the fastest game object in the queue.
             * If the queue is empty, it is rebuilt during this phase.
             */

            // This section handles event subscriptions

            /**
             * The following events must be subscribed and unsubscribed from each turn:
             * - OnMove
             * - OnAttack
             */

            if (selectedGameObject != null)
            {
                selectedGameObject.PositionChanged -= OnMove;
                selectedGameObject.OnAttack -= OnAttack;
            }
            selectedGameObject = turn.Dequeue();

            selectedGameObject.OnAttack += OnAttack;
            selectedGameObject.PositionChanged += OnMove;

            // If the turn order is empty, rebuild it.
            if (turn.Size <= 0)
            {
                Logger.Report(this, "Turnqueue rebuild triggered");
                turn.Enqueue(gameObjects);
            }

            // Start by updating the hud
            UpdateHud();

            // And then the turn console
            UpdateTurnConsole();

            // Copy in the new controls
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
                controls.Controls.Clear();
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
            hud.Clear();
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
            arena.RenderPattern(args.NewValue);

            // tell the arena to update all its positions
            arena.UpdatePositions();
        }

        private void OnAttack(object? sender, GameEvent args)
        {
            // Get the first target out of the game event
            IEnumerable<GameObject> targets = args.GetData<IEnumerable<GameObject>>("targets");

            foreach(GameObject target in targets)
                Logger.Report(sender, $"Attacked {target}");
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

                case 'j':
                    Logger.Report(this, "j pressed");
                    if(arena.HasCachedPattern)
                    {
                        arena.RenderPattern(selectedGameObject.Position);
                    }
                    else
                    {
                        Pattern p = new();
                        p.Mark(0, 0);
                        p.Mark(1, 0);
                        arena.RenderPattern(p, selectedGameObject.Position);
                    }

                    break;

                case 'l':
                    Pattern p2 = new();
                    p2.Mark(0, 0);
                    p2.Mark(1, 0);
                    ExecuteAttack(selectedGameObject, p2);

                    break;
            }
        }

        private void ProcessCombatEvent(CombatEvent e)
        {
            // check the type of combat event before processing.
            switch(e.EventType)
            {
                case CombatEventType.INFO:
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
                Logger.Report(this, $"Game event received is key pressed event with hashcode {e.GetHashCode()}");
            }

            if(e is CombatEvent)
            {
                ProcessCombatEvent(e as CombatEvent);
                Logger.Report(this, $"Game event received is combat event with hashcode {e.GetHashCode()}");
            }

            if(sender is IControllable)
            {
                Logger.Report(this, "Received game event from controllable object");
            }

            UpdateHud();
        }
    }
}
