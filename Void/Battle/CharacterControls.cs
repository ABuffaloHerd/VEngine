using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Event;

namespace Void.Battle
{
    public class CharacterControls : ControlsConsole
    {
        public Action<IGameEvent> Callback;

        public CharacterControls(int width, int height, string name) : base(width, height)
        {
            CreateDefaultControls();
        }

        private void CreateDefaultControls()
        {
            // Name label
            Label nameLabel = new("name")
            {
                Position = new(0, 0)
            };

            // move controls
            Button moveUp = new Button(7)
            {
                Text = "Up",
                Position = new(20, 0),
            };

            Button moveDown = new Button(7)
            {
                Text = "DOwn",
                Position = new(20, 2),
            };

            Button moveLeft = new(7)
            {
                Text = "Left",
                Position = new(13, 1)
            };

            Button moveRight = new Button(7)
            {
                Text = "Right",
                Position = new(27, 1),
            };

            Controls.Add(moveUp);
            Controls.Add(moveDown);
            Controls.Add(moveLeft);
            Controls.Add(moveRight);
        }
    }
}
