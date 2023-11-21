using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Logging;
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

        private GameManager()
        {
            // Singleton pattern
            if (Instance != null)
                throw new Exception("Only one GameManager instance allowed");

            // Subscribe to the scene manager's scene change event
            SceneManager.Instance.OnSceneChanged += HandleSceneChange;

            System.Console.WriteLine("GameManager initialized!");
        }

        /// <summary>
        /// Changes scene and properly subscribes and unsubscribes to the correct objects.
        /// </summary>
        /// <param name="newScene">new scene</param>
        public void HandleSceneChange(Scene newScene)
        {
            // Unsubscribe from the old scene's event
            Scene currentScene = Game.Instance.Screen as Scene;
            if (currentScene != null)
            {
                currentScene.RaiseEvent -= ProcessEvent;
            }

            // Subscribe to the new scene's event
            newScene.RaiseEvent += ProcessEvent;

            // Set currentscene to this current scene
            this.currentScene = newScene;
        }

        /// <summary>
        /// Public method to send game manager an event without subscribing to anything.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        public void SendGameEvent(object? sender, IGameEvent e)
        {
            ProcessEvent(sender, e);
        }

        public override string ToString()
        {
            return "GameManager";
        }

        /// <summary>
        /// Processes an incoming game event, sent by a scene via the RaiseEvent event property.
        /// Game events should be dispatched to the correct manager.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void ProcessEvent(object sender, IGameEvent e)
        {
            if (e is KeyPressedEvent)
                Logger.Report(this, $"Received keyboard press {((KeyPressedEvent)e).Key}");

            // Choose which manager to forward events to
            switch(e.Target)
            {
                case EventTarget.SCENE_MANAGER:
                    // send to scene manager
                    Logger.Report(this, "Forwarded event to Scene Manager!");
                    SceneManager.Instance.HandleEvent(e);
                    break;

                case EventTarget.CURRENT_SCENE:
                    Logger.Report(this, "Forwarded event to current scene!");
                    Event.Invoke(this, e);
                    break;

                case EventTarget.CHARA_MANAGER:
                    // send to character manager
                    break;

                default:
                    // it belongs here. do nothing.
                    break;
            }
        }
    }

    public enum GameState
    {
        TITLE,
        MENU
    }
}
