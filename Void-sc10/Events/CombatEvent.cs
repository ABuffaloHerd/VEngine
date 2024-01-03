using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Logging;

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

        /// <summary>
        /// For attacks
        /// </summary>
        ATTACK,

        /// <summary>
        /// For taking damage
        /// </summary>
        DAMAGED,

        /// <summary>
        /// For creating things
        /// </summary>
        SUMMON,

        /// <summary>
        /// Cast a spell
        /// </summary>
        CAST
    }
    public class CombatEvent : GameEvent, IGameEvent
    {
        public override EventTarget Target => EventTarget.CURRENT_SCENE;

        public CombatEventType EventType { get; set; }

        private static string[] attackFields =
        {
            "amount",
            "source",
            "weapon",
            "targets"
        };

        private static string[] actionFields =
        {
            "action"
        };

        private static string[] speechFields =
        {
            "character",
            "words"
        };

        private static string[] damagedFields =
        {
            "me",
            "amount"
        };

        private static string[] summonFields =
        {
            "summon"
        };

        private static string[] castFields =
        {
            "spell"
        };

        private CombatEvent()
        {
            data = new();
            EventType = CombatEventType.INFO; // default type is info
        }

        private CombatEvent(CombatEventType eventType, Dictionary<string, object> data)
        {
            EventType = eventType;
            this.data = data;
        }

        public static CombatEvent Create(CombatEventType type, Dictionary<string, object> data)
        {
            CombatEvent returnme = new(type, data);
            returnme.Validate();
            return returnme;
        }

        /// <summary>
        /// List of mandatory fields: <br></br>
        /// DAMAGED:<br></br>
        /// amount of damage - "amount"<br></br>
        /// name of source   - "source"<br></br>
        /// name of weapon attacked with - "weapon"<br></br>
        /// list of targets  - "targets"<br></br><br></br>
        /// 
        /// ACTION:<br></br>
        /// name of action - "action"<br></br><br></br>
        /// 
        /// SUMMON: <br></br>
        /// thing to summon - "summon"<br></br><br></br>
        /// 
        /// 
        /// SPEECH:<br></br>
        /// name of character - "character"<br></br>
        /// words to say      - "words"<br></br>
        /// </summary>
        private void Validate()
        {
            // Throw exception or handle missing mandatory fields
            switch (EventType)
            {
                case CombatEventType.ATTACK:
                    foreach (string field in attackFields)
                    {
                        if (!data.ContainsKey(field))
                            throw new InvalidOperationException($"Missing mandatory field {field} in info type combat event!");
                    }
                    break;

                case CombatEventType.DAMAGED:
                    foreach (string field in damagedFields)
                    {
                        if (!data.ContainsKey(field))
                            throw new InvalidOperationException($"Missing mandatory field {field} in info type combat event!");
                    }
                    break;

                case CombatEventType.ACTION:
                    foreach (string field in actionFields)
                    {
                        if (!data.ContainsKey(field))
                            throw new InvalidOperationException($"Missing mandatory field {field} in action type combat event!");
                    }
                    break;

                case CombatEventType.SPEECH:
                    foreach (string field in speechFields)
                    {
                        if (!data.ContainsKey(field))
                            throw new InvalidOperationException($"Missing mandatory field {field} in speech type combat event!");
                    }
                    break;

                case CombatEventType.SUMMON:
                    foreach (string field in summonFields)
                    {
                        if (!data.ContainsKey(field))
                            throw new InvalidOperationException($"Missing mandatory field {field} in speech type combat event!");
                    }
                    break;

                case CombatEventType.CAST:
                    foreach (string field in castFields)
                    {
                        if (!data.ContainsKey(field))
                            throw new InvalidOperationException($"Missing mandatory field {field} in speech type combat event!");
                    }
                    break;

                case CombatEventType.INFO:
                    if(!data.ContainsKey("content"))
                        throw new InvalidOperationException($"Missing mandatory field \"content\" in speech type combat event!");
                    break;
            }

            Logger.Report(this, "Validation for combat event passed");
        }
    }
}
