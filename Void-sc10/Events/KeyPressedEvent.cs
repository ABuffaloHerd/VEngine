using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Events
{
    public class KeyPressedEvent : IGameEvent
    {
        public EventTarget Target => EventTarget.CURRENT_SCENE;

        /// <summary>
        /// Friendly char representation
        /// </summary>
        public char Key { get; private set; }

        /// <summary>
        /// Creates event given char
        /// </summary>
        /// <param name="key"></param>
        public KeyPressedEvent(char key)
        {
            Key = key;
        }

        public IGameEvent AddData(string key, object value)
        {
            this.Key = key[0];

            return this;
        }

        public bool Contains(string key)
        {
            if (key[0] == this.Key) return true;
            else return false;
        }

        public T GetData<T>(string key)
        {
            throw new NotImplementedException();
        }

    }
}
