using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Events
{
    public enum CombatEventType
    {
        /// <summary>
        /// Should be logged to the fight feed or console.
        /// Must contain the amount of damage dealt
        /// </summary>
        INFO,

        /// <summary>
        /// Take an action
        /// </summary>
        ACTION,

        /// <summary>
        /// something funny was said
        /// </summary>
        SPEECH,
    }
    public class CombatEvent : GameEvent, IGameEvent
    {
        public override EventTarget Target => EventTarget.CURRENT_SCENE;

        public CombatEventType EventType { get; set; }

        private Dictionary<string, object> data;

        public CombatEvent()
        {
            data = new();
            EventType = CombatEventType.INFO; // default type is info
        }
    }
}
