using SadConsole;
using SadConsole.Input;
using SadTutorial.Colour;

namespace SadTutorial.IO
{
    public static class ConsoleHelper
    {
        public static void PrintClickable(this Console instance, int x, int y, string str, Action OnClick, string ID)
        {
            instance.PrintClickable(x, y, new ColoredString(str), OnClick, ID);
        }

        public static void PrintClickable(this Console instance, int x, int y, ColoredString str, Action OnClick, string ID)
        {
            Point mousePos = new MouseScreenObjectState(instance, GameHost.Instance.Mouse).CellPosition;

            if (mousePos.X >= x && mousePos.X < x + str.Length && mousePos.Y == y)
            {
                instance.Print(x, y, ColourManipulation.MakeStringDarker(str));
            }
            else
            {
                instance.Print(x, y, str);
            }

            if (GameHost.Instance.Mouse.LeftClicked)
            {
                if (mousePos.X >= x && mousePos.X < x + str.Length && mousePos.Y == y)
                {
                    OnClick();
                }
            }
        }
    }
}
