﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SadConsole;
using VEngine.Events;

namespace VEngine.Scenes
{
    /// <summary>
    /// Usage: Make consoles as needed and call Children.Add(console) in derived classes.
    /// </summary>
    public abstract class Scene : ScreenSurface
    {
        // Local copy of reference to game manager. 
        // The scene will subscribe to the game manager's event in the constructor.
        // This is so that data can be passed between multiple scenes or objects.
        protected GameManager gmInstance = GameManager.Instance;

        // Event to be sent to the game manager. The game manager will subscribe to this event for processing.
        public event EventHandler<IGameEvent> RaiseEvent;

        public Scene() : base(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
        {
            Position = new(0, 0);

            // Tell the game manager that this is its current scene
            gmInstance.HandleSceneChange(this);

            // Subscribe to the GameManager's event dispatch to receive events from it.
            gmInstance.Event += ProcessGameEvent;
        }

        /// <summary>
        /// Sends an event to the game manager.
        /// </summary>
        /// <param name="e">Event to send</param>
        protected void RaiseGameEvent(IGameEvent e)
        {
            RaiseEvent?.Invoke(this, e);
        }

        /// <summary>
        /// Processes a game event sent from the game manager.
        /// Intended for battle interactions
        /// </summary>
        /// <param name="e">Event to process</param>
        protected virtual void ProcessGameEvent(object sender, IGameEvent e)
        {
            System.Console.WriteLine("Warning: Scene.ProcessGameEvent should only be used in a battle scenario and should be overridden.");
        }

    }
}