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
        private ControlsConsole fightFeed;

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

            AddGameObject(test);
            AddGameObject(test2);
            AddGameObject(test3);
            AddGameObject(test4);
            AddGameObject(test5);

            Update();
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            arena.EntityManager.Add(gameObject);

            turn.Enqueue(gameObject);

            turn.Sort();
        }

        private void Update()
        {
            Logger.Report(this, "Update!");
            // Sort, Pop, Turn, End

            /* The sort phase makes appropriate changes to the turn order based on previous events.
             * This means that if an entity's speed is changed in the previous turn, it will be reflected
             * during this phase.
             */
            turn.Sort();


            /* The Pop phase selects the fastest game object in the queue.
             * If the queue is empty, it is rebuilt during this phase.
             */
            selectedGameObject = turn.Dequeue();
            UpdateTurnConsole();

            // If the turn order is empty, rebuild it.
            if (turn.Size <= 0)
            {
                Logger.Report(this, "Turnqueue rebuild triggered");
                turn.Enqueue(gameObjects);
            }
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

        protected override void ProcessGameEvent(object sender, IGameEvent e)
        {
            if(e is KeyPressedEvent)
            {
                KeyPressedEvent kp = e as KeyPressedEvent;
                switch(kp.Key)
                {
                    case 'w':
                        selectedGameObject.Position += new Point(0, -1);
                        break;

                    case 'a':
                        selectedGameObject.Position += new Point(-1, 0);
                        break;

                    case 's':
                        selectedGameObject.Position += new Point(0, 1);
                        break;

                    case 'd':
                        selectedGameObject.Position += new Point(1, 0);
                        break;

                    case ' ':
                        Update();
                        break;
                }
            }
        }
    }
}
