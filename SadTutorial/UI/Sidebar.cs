using SadTutorial.IO;
using SadConsole.Input;

namespace SadTutorial.UI
{
    internal class Sidebar : UI
    {
        int counter = 0;
        bool switch1 = false;
        bool switch2 = false;
        bool clickswitch = false;

        public Sidebar(int width, int height) : base(width, height, "Sidebar", "")
        {
            Window.IsVisible = true;
            Window.Position = new(0, 0);
            Window.CanDrag = false;
        }
        public override void Update()
        {
            if (KeyboardInputHelper.KeyPressed(Keys.Space))
                switch1 = !switch1;

            if (KeyboardInputHelper.HotkeyDown(Keys.Space))
                switch2 = !switch2;

            Console.Print(0, 1, "Switch 1 (Spacebar): " + switch1.ToString());
            Console.Print(0, 2, "Switch 2 (Spacebar Hotkey): " + switch2.ToString());
            Console.PrintClickable(0, 3, "Click Switch: " + clickswitch.ToString() + " [" + counter + " clicks]", UI_Click, "clickedSwitch");

            Console.Clear();
            Console.Print(0, 0, "Fuckinwork");
        }

        public override void Input()
        {
        }
        public override void UI_Click()
        {
            clickswitch = !clickswitch;
            counter++;

            base.UI_Click();
        }
    }
}
