using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.DataStructures;

namespace Void.Event
{
    public enum EventType
    {
        // Scene events (Legacy)
        DEBUG,
        NEW_GAME,
        LOAD_GAME,
        QUIT,
        CHANGE_MENU,
        CHANGE_SCENE,

        // UI events
        UI_BUTTON_CLICK,
        UI_LISTBOX_SELECTION_CHANGED,

        // Arena events
        ARENA_MOVE,

        // "This event type doesn't matter" TODO: I'M GONNA GET RID OF YOU DIRTY DAN
        IDC,

        SCENE
    }

    public interface IGameEvent
    {
        EventType EventType { get; }
        // For chaining
        IGameEvent AddData(string key, object value);
        bool Contains(string key);
        T GetData<T>(string key);
    }

    /// <summary>
    /// Generic game event
    /// </summary>
    public class GameEvent : IGameEvent
    {
        public EventType EventType { get; private set; }
        public Dictionary<string, object> Data { get; private set; }

        public GameEvent(EventType eventType)
        {
            EventType = eventType;
            Data = new Dictionary<string, object>();
        }

        public IGameEvent AddData(string key, object value)
        {
            Data[key] = value;

            return this;
        }

        public T GetData<T>(string key)
        {
            return (T)Data[key];
        }

        public bool Contains(string key)
        {
            return Data.ContainsKey(key);
        }

        public override string ToString()
        {
            return EventType.ToString() + string.Join(", ", Data.Select(kvp => $"{kvp.Key} : {kvp.Value}"));
        }
    }

    /// <summary>
    /// Stores a set of keys.
    /// </summary>
    public sealed class SceneEvent : IGameEvent
    {
        public EventType EventType { get; private set; }
        private HashSet<object> values;

        public SceneEvent(string name)
        {
            EventType = EventType.SCENE;
            values = new();

            AddData(name, 0);
        }

        public IGameEvent AddData(string key, object value)
        {
            values.Add(key); // discard value i don't need it

            return this;
        }

        public T GetData<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string key)
        {
            return values.Contains(key);
        }
    }

    /// <summary>
    /// This event class is a wrapper for a single key value.
    /// </summary>
    public sealed class SingleEvent : IGameEvent
    {
        public EventType EventType { get; private set; }
        private object value;

        public SingleEvent(EventType type, object data) 
        {
            EventType = type;
            value = data;
        }

        /// <summary>
        /// This version of the function overwrites the stored data. Be careful!
        /// </summary>
        /// <param name="key">Unused</param>
        /// <param name="value">The value to store in this wrapper class</param>
        /// <returns></returns>
        public IGameEvent AddData(string key, object value)
        {
            this.value = value;

            return this;
        }

        /// <summary>
        /// Tries its best to compare the string key to whatever lies inside this object.
        /// </summary>
        /// <param name="key">key to compare</param>
        /// <returns>Does this object contain what you're looking for?</returns>
        public bool Contains(string key)
        {
            try
            {
                return ((string)value).Equals(key);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// It doesn't matter what the key is because it only returns one value
        /// </summary>
        /// <typeparam name="T">Type to retrieve</typeparam>
        /// <param name="key">gibberish</param>
        /// <returns>Value stored in this object</returns>
        public T GetData<T>(string key)
        {
            return (T)value;
        }
    }
}
