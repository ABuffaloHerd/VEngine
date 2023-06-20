using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.DataStructures;

namespace Void.Event
{
    public enum EventType
    {
        DEBUG,
        NEW_GAME,
        LOAD_GAME,
        QUIT,
        CHANGE_MENU,
        CHANGE_SCENE
    }

    public class GameEvent
    {
        public EventType EventType { get; } 
        public GameEventData EventData { get; }

        public GameEvent(EventType eventType, GameEventData eventData)
        {
            EventType = eventType;
            EventData = eventData;
        }
    }
}
