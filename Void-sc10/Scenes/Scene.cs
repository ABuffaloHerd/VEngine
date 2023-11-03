using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SadConsole;
using VEngine.Events;

namespace VEngine.Scenes
{
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
        }

        /// <summary>
        /// Sends an event to the game manager.
        /// </summary>
        /// <param name="e">Event to send</param>
        protected void RaiseGameEvent(IGameEvent e)
        {
            System.Console.WriteLine("RaiseGameEvent called!");
            RaiseEvent?.Invoke(this, e);
        }
    }
}
