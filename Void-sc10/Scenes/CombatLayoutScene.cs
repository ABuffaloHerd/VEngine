using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Scenes
{
    /// <summary>
    /// Demo scene for combat UI layout
    /// </summary>
    public class CombatLayoutScene : Scene
    {
        public CombatLayoutScene()
        {
            Console title = new(32, 1)
            {
                Position = new(32, 0)
            };
            title.Surface.DefaultBackground = Color.HotPink;
            title.Print(0, 0, "TOP TEXT");

            Console bgm = new(32, 1)
            {
                Position = new Point(32, 69)
            };
            bgm.Print(0, 0, "BGM");

            Console arena = new(64, 64)
            {
                Position = new(32, 4)
            };
            arena.Surface.DefaultBackground = Color.Red;
            arena.Surface.DefaultForeground = Color.Black;
            arena.Surface.FillWithRandomGarbage(Game.Instance.DefaultFont);
            Border.BorderParameters b = Border.BorderParameters.GetDefault().AddTitle("ARENA");
            new Border(arena, b);

            ControlsConsole controls = new(29, 18)
            {
                Position = new(1, 53)
            };
            SetupControls(controls);

            ControlsConsole hud = new(29, 18)
            {
                Position = new(1, 33)
            };
            hud.Print(0, 0, "HUD");
            b = Border.BorderParameters.GetDefault().AddTitle("STATS");
            new Border(hud, b);
            SetupSampleHud(hud);

            ControlsConsole party = new(29, 30)
            {
                Position = new(1, 1)
            };
            b = Border.BorderParameters.GetDefault().AddTitle("PARTY");
            party.Print(0, 0, "PARTY");
            new Border(party, b);


            Console focus = new(13, 30)
            {
                Position = new(114, 1)
            };
            focus.Surface.DefaultBackground = Color.AnsiBlue;
            focus.Print(0, 0, "Focus");
            b = Border.BorderParameters.GetDefault().AddTitle("FOCUS");
            new Border(focus, b);

            ControlsConsole fightFeed = new(29, 38)
            {
                Position = new(98, 33)
            };
            SetupFightFeed(fightFeed);

            Console turnOrder = new(14, 30)
            {
                Position = new(98, 1)
            };
            b = Border.BorderParameters.GetDefault().AddTitle("TURN");
            new Border(turnOrder, b);
            turnOrder.Print(0, 0, "Turn stack");

            Children.Add(arena);
            Children.Add(title);
            Children.Add(hud);
            Children.Add(controls);
            Children.Add(party);
            Children.Add(fightFeed);
            Children.Add(bgm);
            Children.Add(turnOrder);
            Children.Add(focus);
        }

        private void SetupSampleHud(ControlsConsole target)
        {
            ProgressBar hpBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = 1f,
                Position = new(5, 0)
            };
            Colors theme = new();
            hpBar.BarColor = Color.Red;

            target.Surface.Print(0, 0, "HP: ");
            target.Surface.Print(0, 1, "250 / 250");
            target.Controls.Add(hpBar);

            ProgressBar mpBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = 1f,
                Position = new(5, 3)
            };
            mpBar.BarColor = Color.Purple;
            mpBar.DisplayText = "Overload!";

            target.Surface.Print(0, 3, "MP: ");
            target.Surface.Print(0, 4, "150 / 100");
            target.Controls.Add(mpBar);

            ProgressBar spBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = 0.2f,
                Position = new(5, 6)
            };
            spBar.BarColor = Color.Green;
            target.Surface.Print(0, 6, "SP: ");
            target.Surface.Print(0, 7, "10 / 50");
            target.Controls.Add(spBar);

            ProgressBar odBar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = 0.9f,
                Position = new(5, 9),
                BarGlyph = 177
            };
            odBar.BarColor = Color.Tomato;
            target.Surface.Print(0, 9, "OD: ");
            target.Surface.Print(0, 10, "90 / 100");
            target.Controls.Add(odBar);
        }

        private void SetupFightFeed(ControlsConsole target)
        {
            Border.BorderParameters b = Border.BorderParameters.GetDefault().AddTitle("PARTY");
            b.AddTitle("Fight Feed");

            new Border(target, b);
        }

        private void SetupControls(ControlsConsole target)
        {
            // Buttons for spells, specials and overdrives


            target.Controls.ThemeColors = Colors.CreateSadConsoleBlue();
            var b = Border.BorderParameters.GetDefault().AddTitle("CONTROLS");
            new Border(target, b);
        }
    }
}
