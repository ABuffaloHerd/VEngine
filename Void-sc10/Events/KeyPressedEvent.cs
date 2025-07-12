using SadConsole.Input;
using System;

namespace VEngine.Events
{
    public class KeyPressedEvent : IGameEvent
    {
        public EventTarget Target => EventTarget.CURRENT_SCENE;

        /// <summary>
        /// Friendly char representation
        /// </summary>
        public char Key { get; private set; }
        public Keys SadKey { get; private set; }

        /// <summary>
        /// Creates event given char
        /// </summary>
        /// <param name="key"></param>
        public KeyPressedEvent(char key)
        {
            Key = key;
        }

        public KeyPressedEvent(char key, Keys sadKey) : this(key)
        {
            SadKey = sadKey;
        }

        public IGameEvent AddData(string key, object value)
        {
            this.Key = key[0];
            return this;
        }

        public bool Contains(string key)
        {
            return key[0] == this.Key;
        }

        public T GetData<T>(string key)
        {
            throw new NotImplementedException("KeyPressedEvent does not support generic data access");
        }

        public T GetData<T>(string key, T defaultValue)
        {
            return defaultValue;
        }

        public bool TryGetData<T>(string key, out T value)
        {
            value = default(T)!;
            return false;
        }
    }
}
