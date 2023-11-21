using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Events
{
    /// <summary>
    /// Enum to specify which manager the event should be forwarded to.
    /// </summary>
    public enum EventTarget
    {
        GAME_MANAGER,
        SCENE_MANAGER,
        CHARA_MANAGER,
        CURRENT_SCENE,
        GLOBAL // send to everyone
    }

    public interface IGameEvent
    {
        EventTarget Target { get; }
        IGameEvent AddData(string key, object value);
        bool Contains(string key);
        T GetData<T>(string key);
    }
}
