using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Events
{
    public class CombatEvent : GameEvent, IGameEvent
    {
        public override EventTarget Target => EventTarget.CURRENT_SCENE;

        private Dictionary<string, object> data;

        public CombatEvent()
        {
            data = new();
        }
    }
}
