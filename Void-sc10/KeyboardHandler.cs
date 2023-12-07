using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;

using SadConsole.Input;

using gm = VEngine.GameManager;

namespace VEngine
{
    public static class KeyboardHandler
    {
        private static Dictionary<Keys, Action> registeredKeys = new();

        static KeyboardHandler()
        {
            // Register keys here //

            RegisterKey(Keys.W, 'w');
            RegisterKey(Keys.A, 'a');
            RegisterKey(Keys.S, 's');
            RegisterKey(Keys.D, 'd');

            RegisterKey(Keys.J, 'j');
            RegisterKey(Keys.Q, 'q');
            RegisterKey(Keys.L, 'l');

            RegisterKey(Keys.Space, (char)32);

            // Arrow keys
            RegisterKey(Keys.Up,    (char)38);
            RegisterKey(Keys.Down,  (char)40);
            RegisterKey(Keys.Left,  (char)37);
            RegisterKey(Keys.Right, (char)39);

        }

        public static void HandleKeyboard(object? sender, GameHost host)
        {
            // pick one of these registered keys and send it to the game manager for dispatch.
            foreach (var key in registeredKeys.Keys)
            {
                if (Game.Instance.Keyboard.IsKeyPressed(key))
                    registeredKeys[key]?.Invoke();
            }
        }

        /// <summary>
        /// Helper method to register a key
        /// </summary>
        /// <param name="k">Key to register</param>
        /// <param name="c">Char representation of key to register</param>
        private static void RegisterKey(Keys k, char c)
        {
            registeredKeys[k] = () => gm.Instance.SendGameEvent(null, new KeyPressedEvent(c));
        }
    }
}
