using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;

namespace VEngine.Factory
{
    public class CombatEventBuilder
    {
        private CombatEventType eventType;
        private Dictionary<string, object> data = new();

        public CombatEventBuilder SetEventType(CombatEventType type)
        {
            eventType = type;
            return this;
        }

        // Methods for mandatory fields
        public CombatEventBuilder AddField(string key, object value)
        {
            data[key] = value;
            return this;
        }

        public CombatEvent Build()
        {
            return CombatEvent.Create(eventType, data);
        }


    }
}
