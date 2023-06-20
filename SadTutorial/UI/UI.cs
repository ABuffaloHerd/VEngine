using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using System.Xml;

namespace SadTutorial.UI
{
    public abstract class UI
    {
        public Console Console;
        public Window Window;

        public UI(int width, int height, string ID, string title = "")
        {
            Window = new(width, height);
            Window.CanDrag = false;
            Window.Position = new((Program.Width - width) / 2, (Program.Height - height) / 2);

            int conWidth = width - 2;
            int conHeight = height - 2;

            Console = new(conWidth, conHeight);
            Console.Position = new(1, 1);
            Window.Title = title.Align(HorizontalAlignment.Center, conWidth, (char)196);

            Window.Children.Add(Console);
            Program.UIManager.Children.Add(Window);

            Window.Show();
            Window.IsVisible = false;

            Program.UIManager.Interfaces.Add(ID, this);
        }

        public virtual void Update()
        {
            Console.Clear();
        }

        public virtual void Input()
        {
            Point mpos = new MouseScreenObjectState(Console, GameHost.Instance.Mouse).CellPosition;
        }

        public virtual void UI_Click()
        {
            // wtf???
        }
    }
}
