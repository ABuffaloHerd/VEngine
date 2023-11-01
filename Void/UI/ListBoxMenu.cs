using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Event;

namespace Void.UI
{
    // Menu that uses a listbox system.
    public class ListBoxMenu : Menu
    {
        private int height;
        private int width;
        public ListBoxMenu(int width, int height, Dictionary<string, IGameEvent> keyValuePairs, string title = "") : base(width, height, keyValuePairs, title)
        {
            this.height = keyValuePairs.Count;
            this.width = width;
            Render();
        }

        public override void Render()
        {
            // rendering
            ListBox lb = new(30, this.height + 2)
            {
                DrawBorder = true,
                BorderLineStyle = (int[])ICellSurface.ConnectedLineThick.Clone(),
                Position = new(0, 0)
            };

            foreach (var kvp in menuOptions)
            {
                lb.Items.Add(kvp.Key);
            }

            lb.SelectedItemChanged += (s, a) =>
            {
                OnCallback(menuOptions[lb.SelectedItem.ToString()]);
            };

            Controls.Add(lb);
        }
    }
}
