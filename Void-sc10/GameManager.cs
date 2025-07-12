using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VEngine.Events;
using VEngine.Logging;
using VEngine.Objects;
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
        
        // Efficient event routing using delegates
        private readonly Dictionary<EventTarget, Action<IGameEvent>> eventHandlers;

        private GameManager()
        {
            // Singleton pattern
            if (Instance != null)
                throw new Exception("Only one GameManager instance allowed");

            // Initialize event handlers
            eventHandlers = new Dictionary<EventTarget, Action<IGameEvent>>
            {
                { EventTarget.SCENE_MANAGER, HandleSceneManagerEvent },
                { EventTarget.CURRENT_SCENE, HandleCurrentSceneEvent },
                { EventTarget.CHARA_MANAGER, HandleCharacterManagerEvent },
                { EventTarget.GLOBAL, HandleGlobalEvent }
            };

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
            if (currentScene != null)
            {
                currentScene.RaiseEvent -= ProcessEvent;
                Logger.Report(this, "Unsubscribed from scene event");
            }

            // Subscribe to the new scene's event
            newScene.RaiseEvent += ProcessEvent;
            Logger.Report(this, "Subscribed to new scene's event");

            // Set currentscene to this current scene
            currentScene = newScene;
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

            // Use efficient delegate-based routing instead of switch statement
            if (eventHandlers.TryGetValue(e.Target, out var handler))
            {
                handler(e);
            }
        }

        private void HandleSceneManagerEvent(IGameEvent e)
        {
            Logger.Report(this, "Forwarded event to Scene Manager!");
            SceneManager.Instance.HandleEvent(e);
        }

        private void HandleCurrentSceneEvent(IGameEvent e)
        {
            Logger.Report(this, "Forwarded event to current scene!");
            Event?.Invoke(this, e);
        }

        private void HandleCharacterManagerEvent(IGameEvent e)
        {
            // TODO: Implement character manager event handling
            // CharacterManager.Instance.HandleEvent(e);
        }

        private void HandleGlobalEvent(IGameEvent e)
        {
            // Send to all handlers
            HandleSceneManagerEvent(e);
            HandleCurrentSceneEvent(e);
            HandleCharacterManagerEvent(e);
        }

        /// <summary>
        /// Cleanup method to prevent memory leaks
        /// </summary>
        public void Dispose()
        {
            if (currentScene != null)
            {
                currentScene.RaiseEvent -= ProcessEvent;
                currentScene = null;
            }
            
            SceneManager.Instance.OnSceneChanged -= HandleSceneChange;
        }
    }

    public enum GameState
    {
        TITLE,
        MENU
    }
}
