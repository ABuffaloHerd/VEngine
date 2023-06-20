using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Event;
using Void.Scene;

namespace Void
{
    public partial class GameManager
    {
        public event Action<GameEvent> Event;
        private GameState state;
        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
        }

        public void Raise(GameEvent e)
        {
            Event.Invoke(e);
        }
        public void Process(GameEvent e)
        {
            System.Console.WriteLine($"Event Type: {e.EventType}, Event Data: {e.EventData}");

            switch(e.EventType)
            {
                case EventType.NEW_GAME:
                    System.Console.WriteLine("New game event");
                    state = GameState.MAINMENU;
                    SwitchScene(new MainMenu());
                    break;

                case EventType.CHANGE_MENU:
                    if(e.EventData.Get("data").Equals("title"))
                    {
                        System.Console.WriteLine("Title return");
                        state = GameState.MAINMENU;
                        SwitchScene(new TitleScreen());
                    }
                    break;
            }
        }
    }

    enum GameState
    {
        TITLE,
        MAINMENU,
        MAP,
        BATTLE,
        SHOP,
        INVENTORY
    }
}
