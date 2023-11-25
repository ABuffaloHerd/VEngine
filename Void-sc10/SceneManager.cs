using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Logging;
using VEngine.Scenes;
using VEngine.Scenes.Combat;

namespace VEngine
{
    public class SceneManager
    {
        public static SceneManager Instance { get; } = new();

        public Scene CurrentScene { get; private set; }

        // Event for scene changing. This is used for the game manager to run the change scene function to handle subscriptions when scenes change.
        public event Action<Scene> OnSceneChanged;

        private GameManager gmInstance;

        // Register new scenes here //
        private readonly Dictionary<string, Func<Scene>> sceneFactory = new()
        {
            { "test_scene", () => new TestScene() },
            { "title", () => new TitleScene() },
            { "arena_layout", () => new CombatLayoutScene() }
        };

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
            SceneChangeEvent ev = null;

            if (e is SceneChangeEvent)
                ev = e as SceneChangeEvent;

            if(sceneFactory.TryGetValue(ev.TargetScene, out var func)) 
            {
                ChangeScene(func());
            }
            else
            {
                Logger.Report(this, $"No scene registered under {ev.TargetScene}");
            }
        }

        private void ChangeScene(Scene newScene)
        {
            Scene oldscene = Game.Instance.Screen as Scene;
            Game.Instance.Screen = newScene;

            // Trigger the scene change event
            OnSceneChanged?.Invoke(newScene);

            // Dispose of the old scene to stop memory leaks
            oldscene.Dispose();
        }
    }
}
