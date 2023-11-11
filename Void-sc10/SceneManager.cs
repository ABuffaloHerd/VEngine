using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Scenes;

namespace VEngine
{
    public class SceneManager
    {
        public static SceneManager Instance { get; } = new();

        public Scene CurrentScene { get; private set; }

        // Event for scene changing. This is used for the game manager to run the change scene function to handle subscriptions when scenes change.
        public event Action<Scene> OnSceneChanged;

        private GameManager gmInstance;
        /// <summary>
        /// Handles the changing of scenes so that responsibility is off the GameManager.
        /// </summary>
        private SceneManager()
        {
            // No need to fear the null reference assignment because the title scene is already a scene object.
            CurrentScene = Game.Instance.Screen as Scene;

            gmInstance = GameManager.Instance;

            System.Console.WriteLine("Scene Manager initialized!");
        }

        public void HandleEvent(IGameEvent e)
        {
            if(e.Contains("change_scene"))
            {
                // Find out which scene to change to

                string data = e.GetData<string>("change_scene");
                if (data.Equals("test_scene"))
                    ChangeScene(new TestScene());
                else if (data.Equals("title"))
                    ChangeScene(new TitleScene());
                else if (data.Equals("arena_layout"))
                    ChangeScene(new CombatLayoutScene());
            }
        }

        private void ChangeScene(Scene newScene)
        {
            Game.Instance.Screen = newScene;

            // Trigger the scene change event
            OnSceneChanged?.Invoke(newScene);
        }
    }
}
