using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Events
{
    /// <summary>
    /// Encapsulates a dictionary under the IGameEvent interface to emulate json
    /// </summary>
    public class GameEvent : IGameEvent
    {
        protected Dictionary<string, object> data;

        public virtual EventTarget Target
        {
            get => target;
            set => target = value;
        }
        protected EventTarget target = EventTarget.GAME_MANAGER;
        public GameEvent(Dictionary<string, object> data)
        {
            this.data = data;
        }

        public GameEvent(string singledata)
        {
            this.data = new();

            AddData(singledata, 0);
        }

        public GameEvent()
        {
            data = new();
        }

        public IGameEvent AddData(string key, object value)
        {
            data.Add(key, value);

            return this;
        }

        public bool Contains(string key)
        {
            return data.ContainsKey(key);
        }

        public T GetData<T>(string key)
        {
            return (T)data[key];
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            foreach(var kvp in data)
            {
                sb.Append(kvp.Key + " : " + kvp.Value).Append('\n');
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Encapsulates a single string object pair
    /// </summary>
    public class SinglePairEvent : IGameEvent
    {
        private string key;
        private object val;

        public virtual EventTarget Target => EventTarget.GAME_MANAGER;

        public IGameEvent AddData(string key, object value)
        {
            this.key = key;
            this.val = value;

            return this;
        }

        public bool Contains(string key)
        {
            return this.key.Equals(key) || this.val.Equals(key);
        }

        public T GetData<T>(string key)
        {
            return (T)val;
        }
    }
}
