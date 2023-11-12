using SadConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Scenes
{
    public partial class CombatScene : Scene
    {
        Console title;
        Console bgm;
        Console arena; // change to custom class extending entity manager
        Console focus;
        Console turnOrder;

        ControlsConsole controls;
        ControlsConsole hud;
        ControlsConsole party;
        ControlsConsole fightFeed;
        public CombatScene() 
        {
            title = new(32, 1)
            {
                Position = new(32, 0)
            };
            Children.Add(title); 

            bgm = new(32, 1)
            {
                Position = new(32, 69)
            };
            Children.Add(bgm);

            arena = new(64, 64)
            {
                Position = new(32, 4)
            };
            SetupArena();
            Children.Add(arena); 

            controls = new(29, 18)
            {
                Position = new(1, 53)
            };
            // Setup controls
            Children.Add(controls);

            hud = new(29, 18)
            {
                Position = new(1, 33)
            };
            Children.Add(hud);

            party = new(29, 30)
            {
                Position = new(1, 1)
            };
            Children.Add(party);

            focus = new(13, 30)
            {
                Position = new(114, 1)
            };
            Children.Add(focus);

            fightFeed = new(29, 38)
            {
                Position = new(98, 33)
            };
            Children.Add(fightFeed);

            turnOrder = new(14, 30)
            {
                Position = new(98, 1)
            };
            Children.Add(turnOrder);

        }
    }
}
