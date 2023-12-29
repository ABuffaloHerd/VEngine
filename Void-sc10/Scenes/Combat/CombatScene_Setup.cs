using SadConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Scenes.Combat
{
    public partial class CombatScene : Scene
    {
        private void SetupArena()
        {
            // i'm not typing border.borderparameters 20 times
            var b = Border.BorderParameters
                .GetDefault()
                .AddTitle("ARENA");
            new Border(arena, b);
        }
        
        private void SetupFocus()
        {
            var b = Border.BorderParameters
                .GetDefault()
                .AddTitle("FOCUS");
            new Border(focus, b);
        }

        private void SetupFightFeed()
        {
            var b = Border.BorderParameters
                .GetDefault()
                .AddTitle("Fight Feed");

            new Border(fightFeed, b);
        }

        private void SetupTurnOrder()
        {
            var b = Border.BorderParameters
                .GetDefault()
                .AddTitle("TURN");

            new Border(turnOrder, b);
            turnOrder.Surface.UsePrintProcessor = true;  
        }

        private void SetupControls()
        {
            var b = Border.BorderParameters
                .GetDefault()
                .AddTitle("CONTROLS");

            new Border(controls, b);
        }

        private void SetupHud()
        {
            var b = Border.BorderParameters
                .GetDefault()
                .AddTitle("HUD");

            new Border(hud, b);
        }

        private void SetupParty()
        {
            var b = Border.BorderParameters
                .GetDefault()
                .AddTitle("PARTY");

            new Border(party, b);
        }

        private void SetupConsoles()
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

            /// TODO: Make this a parameter
            arena = new(32, 16)
            {
                //Position = new(32, 4)
            };
            Children.Add(arena);

            controls = new(29, 18)
            {
                Position = new(1, 53),
                UseKeyboard = false
            };
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

            fightFeed = new(29, 38);
            Children.Add(fightFeed);

            turnOrder = new(14, 30)
            {
                Position = new(98, 1)
            };
            Children.Add(turnOrder);
        }
    }
}
