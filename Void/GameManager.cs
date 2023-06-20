using Microsoft.Xna.Framework.Audio;
using SadConsole;
using SadConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Void.Event;
using Void.Scene;
using Void.UI;

namespace Void
{
    // singleton gamemanager class
    public partial class GameManager : ScreenObject
    {
        public static GameManager Instance { get; private set; }

        // Width and height of the window in cells
        public int Width { get; }
        public int Height { get; }

        public GameManager()
        {
            if(Instance != null)
            {
                throw new Exception("Only one GameManager instance allowed.");
            }

            Instance = this;
            Position = new(0, 0);

            Width = Game.Instance.ScreenCellsX;
            Height = Game.Instance.ScreenCellsY;

            state = GameState.TITLE;

            // setup event handlers
            Event += Process;

            Children.Add(new TitleScreen());
        }

        private void SwitchScene(BaseScene newScene)
        {
            Children.Clear();
            Children.Add(newScene);
        }
    }
}
