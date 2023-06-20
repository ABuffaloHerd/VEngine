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

        public void Add(string key, object value)
        {
            data.Add(key, value);
        }

        public void Set(string key, object value)
        {
            data[key] = value;
        }

        public object Get(string key)
        {
            return data[key];
        }

        public bool Contains(string key)
        {
            return data.ContainsKey(key);
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
