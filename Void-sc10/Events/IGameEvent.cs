using System;

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
        
        /// <summary>
        /// Type-safe data access with default value
        /// </summary>
        T GetData<T>(string key, T defaultValue);
        
        /// <summary>
        /// Try to get data with type safety
        /// </summary>
        bool TryGetData<T>(string key, out T value);
    }
}
