using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.DataStructures
{
    // JSON emulator
    public struct GameEventData
    {
        private Dictionary<string, object> data;

        public GameEventData()
        {
            data = new();
        }

        public GameEventData(string data) : this()
        {
            this.data.Add("data", data);
        }

        public GameEventData(string key, object value) : this()
        {
            Add(key, value);
        }

        public GameEventData Add(string key, object value)
        {
            data.Add(key, value);

            return this;
        }

        public GameEventData Set(string key, object value)
        {
            data[key] = value;

            return this;
        }

        public T Get<T>(string key)
        {
            return (T)data[key];
        }

        public bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }

        public bool ContainsValue(object thing)
        {
            foreach(var kvp in data)
            {
                if (data[kvp.Key].Equals(thing)) return true;
            }

            return false;
        }

        public int GetInt(string key)
        {
            return (int)Get(key);
        }

        public override string ToString()
        {
            if (data.Count == 0)
                return "Empty Gamedata";

            StringBuilder sb = new();

            foreach(var kvp in data)
            {
                sb.Append(kvp.Key).Append(" : ").Append(kvp.Value);
            }

            return sb.ToString();
        }
    }
}
