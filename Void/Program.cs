using SadConsole;
using SadRogue.Primitives;
using Void;

namespace SadConsoleSetup
{
    internal class Program
    {
        public const int Width = 190;
        public const int Height = 50;
        static void Main(string[] args)
        {
            /////////////////////////////////
            /* SadConsole V10 kickoff code */
            /////////////////////////////////

            Settings.WindowTitle = "SadConsole";

            Game.Configuration gameStartup = new Game.Configuration()
                .SetScreenSize(190, 50)
                .SetStartingScreen<GameManager>()
                .OnStart(OnStart)
                .IsStartingScreenFocused(false)
                .ConfigureFonts((f) => f.UseBuiltinFontExtended());

            Game.Create(gameStartup);
            Game.Instance.Run();
            Game.Instance.Dispose();

            // V9 code

            //// Setup the engine and create the main window.
            //SadConsole.Game.Create(Width, Height);

            //// Hook the start event so we can add consoles to the system.
            //SadConsole.Game.Instance.OnStart = OnStart;

            //// Start the game.
            //SadConsole.Game.Instance.Run();
            //SadConsole.Game.Instance.Dispose();
        }

        private static void OnStart()
        {
            //ColoredGlyph boxBorder = new ColoredGlyph(Color.White, Color.Black, 178);
            //ColoredGlyph boxFill = new ColoredGlyph(Color.White, Color.Black);

            // V9 Only
            //Game.Instance.Screen = new GameManager();
            //Game.Instance.DestroyDefaultStartingConsole();

            //Game.Instance.StartingConsole.FillWithRandomGarbage(255);
            //Game.Instance.StartingConsole.DrawBox(new Rectangle(2, 2, 26, 5), ShapeParameters.CreateFilled(boxBorder, boxFill));
            //Game.Instance.StartingConsole.Print(4, 4, "Welcome to SadConsole!");
        }
    }
}