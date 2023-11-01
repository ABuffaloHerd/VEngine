using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Void.UI;
using Void.Event;

namespace Void.Scene
{
    // represents a collection of IScreenObjects and menus
    public abstract class Scene : ScreenSurface, IRenderable
    {
        public SceneType SceneType { get; protected set; }

        // processes child events
        protected Action<IGameEvent> callback;
        protected GameManager gmInstance;
        public Scene() : base(GameManager.Instance.Width, GameManager.Instance.Height)
        {
            Position = new(0, 0);
            gmInstance = GameManager.Instance;
            SceneType = SceneType.MENU;
            callback += Process;
        }

        public abstract void Render();
        public virtual void Raise(IGameEvent e)
        {
            callback?.Invoke(e);
        }

        protected virtual void Process(IGameEvent e)
        {
            return;
        }
    }

    public enum SceneType
    {
        DEBUG,
        MENU
    }

}
