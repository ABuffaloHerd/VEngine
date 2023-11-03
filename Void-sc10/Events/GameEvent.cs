using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Events
{
    public class GameEvent : IGameEvent
    {
        private Dictionary<string, object> data;

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
                sb.Append(kvp.Key + " " + kvp.Value);
            }

            return sb.ToString();
        }
    }
}
