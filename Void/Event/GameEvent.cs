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

    public class SceneEvent : IGameEvent
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

    // This class contains only one value
    public class SingleEvent : IGameEvent
    {
        public EventType EventType { get; private set; }
        private HashSet<object> values;

        public SingleEvent(EventType type, string data) 
        {
            EventType = type;
            values = new();

            values.Add(data);
        }
    }
}
