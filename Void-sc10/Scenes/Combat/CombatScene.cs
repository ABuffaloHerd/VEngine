using SadConsole.Input;
using SadConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Objects;

namespace VEngine.Scenes.Combat
{
    public partial class CombatScene : Scene
    {
        private Console title;
        private Console bgm;
        private Console focus;
        private Console turnOrder;

        private ControlsConsole controls;
        private ControlsConsole hud;
        private ControlsConsole party;
        private ControlsConsole fightFeed;

        private Arena arena;

        private TurnQueue turn;

        private HashSet<GameObject> gameObjects;
        private HashSet<PlayerGameObject> players;

        public CombatScene() : base()
        {
            turn = new();

            SetupConsoles();
            SetupArena();
            SetupFightFeed();
            SetupControls();
            SetupTurnOrder();
            SetupFocus();
            SetupHud();
            SetupParty();
        }

        public void Update()
        {
            
        }

        protected override void ProcessGameEvent(object sender, IGameEvent e)
        {
            if(e is KeyPressedEvent)
            {
                KeyPressedEvent kp = e as KeyPressedEvent;
                switch(kp.Key)
                {
                    case 'w':
                        arena.Surface.DefaultBackground = Color.AliceBlue;
                        break;

                    case 'a':
                        arena.Surface.DefaultBackground = Color.Red;
                        break;

                }
            }
        }
    }
}
