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
        // Game events
        DEBUG,
        NEW_GAME,
        LOAD_GAME,
        QUIT,
        CHANGE_MENU,
        CHANGE_SCENE,

        // UI events
        UI_BUTTON_CLICK,
        UI_LISTBOX_SELECTION_CHANGED,

        // "This event type doesn't matter"
        IDC
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

        public override string ToString()
        {
            return EventType.ToString() + EventData.ToString();
        }
    }
}
