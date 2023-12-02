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
        private HashSet<PlayerGameObject> players;

        private GameObject selectedGameObject;

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
            aso.CreateFrame()[0].Glyph = 'C';
            PlayerGameObject pgo = new(aso, 2);
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
            arena.EntityManager.Add(gameObject);

            turn.Enqueue(gameObject);

            turn.Sort();
        }

        /// <summary>
        /// Performs a full update of all elements in this scene
        /// </summary>
        private void Update()
        {
            Logger.Report(this, "Update!");

            // Sort, Pop, Turn, End

            /* The sort phase makes appropriate changes to the turn order based on previous events.
             * This means that if an entity's speed is changed in the previous turn, it will be reflected
             * during this phase.
             */

        }

        /// <summary>
        /// Runs when it's a new object's turn
        /// </summary>
        private void OnNextTurn()
        {
            turn.Sort();

            /* The Pop phase selects the fastest game object in the queue.
             * If the queue is empty, it is rebuilt during this phase.
             */
            selectedGameObject = turn.Dequeue();

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

            // Reset the controls console
            SetupControls();

            // Copy in the new controls
            if (selectedGameObject is IControllable)
            {
                IControllable controllable = selectedGameObject as IControllable;

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
            hud.Print(0, 0, selectedGameObject.Name);
            hud.Print(0, 17, $"M: {selectedGameObject.MoveDist}");
        }

        private void ProcessKeyEvent(KeyPressedEvent kpe)
        {
            switch (kpe.Key)
            {
                case 'w':
                    selectedGameObject.Move(new Point(0, -1));
                    break;

                case 'a':
                    selectedGameObject.Move(new Point(-1, 0));
                    break;

                case 's':
                    selectedGameObject.Move(new Point(0, 1));
                    break;

                case 'd':
                    selectedGameObject.Move(new Point(1, 0));
                    break;

                case ' ':
                    OnNextTurn();
                    break;

                case 'j':
                    Logger.Report(this, "j pressed");
                    Pattern p = new();
                    p.Mark(0, 0);
                    p.Mark(1, 0);
                    RenderPattern(p, selectedGameObject.Position);
                    break;
            }
        }

        protected override void ProcessGameEvent(object sender, IGameEvent e)
        {
            if(e is KeyPressedEvent)
            {
                ProcessKeyEvent(e as KeyPressedEvent);
            }

            if(sender is IControllable)
            {
                Logger.Report(this, "Received game event from controllable object");
                fightFeed.Print("A button was pressed");
            }

            UpdateHud();
        }
    }
}
