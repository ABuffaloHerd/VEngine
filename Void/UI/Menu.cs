using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.UI.Controls;
using Void.Event;

namespace Void.UI
{
    // This is a simple menu class that prints out events.
    public class Menu : BaseMenu
    {
        public Menu(int width, int height, Dictionary<string, GameEvent> keyValuePairs, string title = "") : base(width, height, title)
        {
            foreach(var kvp in keyValuePairs)
            {
                Insert(kvp.Key, kvp.Value);
            }

            Render();
        }

        public override void Render()
        {
            int len = GetMaxLen();
            Point pos = new(0, 0);
            foreach(var kvp in menuOptions)
            {
                Button b = new(len + 4)
                {
                    Text = kvp.Key,
                    Position = pos
                };
                b.Click += (s, a) => 
                {
                    gmInstance.Raise(kvp.Value);
                };

                Controls.Add(b);

                pos = new(0, pos.Y + 1);
            }
        }

        // Gets max length of menu options
        private int GetMaxLen()
        {
            int len = 0;
            foreach(var kvp in menuOptions)
            {
                if (kvp.Key.Length > len)
                    len = kvp.Key.Length;
            }

            return len;
        }
    }
}
