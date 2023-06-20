using SadConsole.UI;
using SadTutorial.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadTutorial.UI
{
    // Container for consoles. So it doesn't actually do anything.
    public class UIManager : ScreenObject
    {
        public SadConsole.UI.Colors CustomColors;
        public Dictionary<string, UI> Interfaces;

        public UIManager()
        {
            IsVisible = true;
            IsFocused = true;
            UseMouse = true;
            Parent = GameHost.Instance.Screen;
            Interfaces = new();

            Init();
        }

        public override void Update(TimeSpan timeElapsed)
        {
            foreach(var kvp in Interfaces)
            {
                if(kvp.Value.Window.IsVisible)
                {
                    kvp.Value.Update();
                }
            }

            KeyboardInputHelper.ClearKeys();
            base.Update(timeElapsed);
        }

        public void Init()
        {
            SetupCustomColors();

            Sidebar sb = new(40, 50);
        }

        private void SetupCustomColors()
        {
            CustomColors = SadConsole.UI.Colors.CreateAnsi();
            CustomColors.ControlHostBackground = new AdjustableColor(Color.Black, "Black");
            CustomColors.Lines = new AdjustableColor(Color.White, "White");
            CustomColors.Title = new AdjustableColor(Color.White, "White");

            CustomColors.RebuildAppearances();
            SadConsole.UI.Themes.Library.Default.Colors = CustomColors;
        }
    }
}
