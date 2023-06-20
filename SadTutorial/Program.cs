using SadConsole;
using SadTutorial.UI;
using System.Data;
using System.Xml.Serialization;

namespace SadTutorial
{
    public class Program
    {
        public const int Width = 190;
        public const int Height = 50;
        public const int MapWidth = 70;
        public const int MapHeight = 40;

        public static UIManager UIManager;

        static void Main(string[] args)
        {
            Game.Create(Width, Height, Init);
            
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        static void Init()
        {
            UIManager = new();
            UIManager.Init();

            Game.Instance.MonoGameInstance.Window.Title = "Test";
        }

    }
}
