using SadConsole;
using SadConsole.Input;
using SadConsole.UI;

using Void.Event;

namespace Void.UI
{
    public abstract class BaseMenu : ControlsConsole, IRenderable
    {
        public event Action<GameEvent> Callback;

        protected Dictionary<string, GameEvent> menuOptions;
        protected GameManager gmInstance;

        public BaseMenu(int width, int height, string title = "") : base(width, height)
        {
            menuOptions = new();
            gmInstance = GameManager.Instance;
        }

        public void Insert(string key, GameEvent value)
        {
            menuOptions.Add(key, value);
        }

        // The render function builds the screen to the class specifications.
        public abstract void Render();

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
        }

        protected virtual void OnCallback(GameEvent e)
        {
            Callback?.Invoke(e);
        }
    }
}
