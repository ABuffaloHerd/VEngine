using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Scenes;

namespace VEngine
{
    /// <summary>
    /// This class keeps track of the entire gamestate and global variables
    /// </summary>
    public class GameManager
    {
        public GameState State { get; private set; } = GameState.TITLE;
        public static GameManager Instance { get; } = new();

        /// <summary>
        /// Global event dispatcher.
        /// Classes subscribe to this event to receive events from the game manager.
        /// </summary>
        public event EventHandler<IGameEvent> Event;

        private Scene currentScene;
        public GameManager()
        {
            // Singleton pattern
            if (Instance != null)
                throw new Exception("Only one GameManager instance allowed");

            System.Console.WriteLine("GameManager initialized!");
        }

        /// <summary>
        /// Changes scene and properly subscribes and unsubscribes to the correct objects.
        /// </summary>
        /// <param name="newScene">new scene</param>
        public void ChangeScene(Scene newScene)
        {
            // Unsubscribe from the old scene's event
            Scene currentScene = Game.Instance.Screen as Scene;
            if (currentScene != null)
            {
                currentScene.RaiseEvent -= ProcessEvent;
            }

            // Change the scene
            Game.Instance.Screen = newScene;

            // Subscribe to the new scene's event
            newScene.RaiseEvent += ProcessEvent;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append($"GameState : {State}");

            return sb.ToString();
        }

        /// <summary>
        /// Processes an incoming game event, sent by a scene via the RaiseEvent event property.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void ProcessEvent(object sender, IGameEvent e)
        {
            if(e is GameEvent)
            {
                System.Console.WriteLine(e.ToString());

                // Send a sanity check event
                GameEvent ev = new();
                ev.AddData("test 2", "gamemanager to scene");
                Event.Invoke(this, ev);
            }
        }
    }

    public enum GameState
    {
        TITLE,
        MENU
    }
}
