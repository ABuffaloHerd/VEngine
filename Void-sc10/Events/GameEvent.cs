using System;
using System.Collections.Generic;
using System.Linq;

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
            this.data = data ?? new Dictionary<string, object>();
        }

        public GameEvent(string singledata)
        {
            this.data = new Dictionary<string, object>();
            AddData(singledata, 0);
        }

        public GameEvent()
        {
            data = new Dictionary<string, object>();
        }

        public IGameEvent AddData(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
                
            data[key] = value; // Use indexer to allow overwriting
            return this;
        }

        public bool Contains(string key)
        {
            return !string.IsNullOrEmpty(key) && data.ContainsKey(key);
        }

        public T GetData<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
                
            if (!data.TryGetValue(key, out var value))
                throw new KeyNotFoundException($"Key '{key}' not found in event data");
                
            if (value is T typedValue)
                return typedValue;
                
            throw new InvalidCastException($"Cannot cast value of type {value?.GetType()} to {typeof(T)}");
        }

        /// <summary>
        /// Type-safe data access with default value
        /// </summary>
        public T GetData<T>(string key, T defaultValue)
        {
            if (string.IsNullOrEmpty(key) || !data.TryGetValue(key, out var value))
                return defaultValue;
                
            if (value is T typedValue)
                return typedValue;
                
            return defaultValue;
        }

        /// <summary>
        /// Try to get data with type safety
        /// </summary>
        public bool TryGetData<T>(string key, out T value)
        {
            value = default(T)!;
            
            if (string.IsNullOrEmpty(key) || !data.TryGetValue(key, out var objValue))
                return false;
                
            if (objValue is T typedValue)
            {
                value = typedValue;
                return true;
            }
            
            return false;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, data.Select(kvp => $"{kvp.Key} : {kvp.Value}"));
        }

        public void Clear()
        {
            data.Clear();
        }
    }

    /// <summary>
    /// Encapsulates a single string object pair
    /// </summary>
    public class SinglePairEvent : IGameEvent
    {
        private string key = string.Empty;
        private object val = null!;

        public virtual EventTarget Target => EventTarget.GAME_MANAGER;

        public IGameEvent AddData(string key, object value)
        {
            this.key = key ?? string.Empty;
            this.val = value ?? string.Empty;
            return this;
        }

        public bool Contains(string key)
        {
            return this.key.Equals(key) || (val?.Equals(key) == true);
        }

        public T GetData<T>(string key)
        {
            if (this.key.Equals(key))
                return (T)val;
                
            throw new KeyNotFoundException($"Key '{key}' not found in SinglePairEvent");
        }

        /// <summary>
        /// Type-safe data access with default value
        /// </summary>
        public T GetData<T>(string key, T defaultValue)
        {
            if (this.key.Equals(key) && val is T typedValue)
                return typedValue;
                
            return defaultValue;
        }

        /// <summary>
        /// Try to get data with type safety
        /// </summary>
        public bool TryGetData<T>(string key, out T value)
        {
            value = default(T)!;
            
            if (this.key.Equals(key) && val is T typedValue)
            {
                value = typedValue;
                return true;
            }
            
            return false;
        }
    }
}
