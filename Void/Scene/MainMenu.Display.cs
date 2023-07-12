using SadConsole.Components;
using SadConsole.Instructions;
using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Event;
using Void.UI;

namespace Void.Scene;

public partial class MainMenu
{
    private static Point DEFAULT_POSITION = new(35, 4);
    private const int DEFAULT_WIDTH = 150;
    private const int DEFAULT_HEIGHT = 30;
    private abstract class BaseMenuDisplay : Console
    {
        protected DrawString instructions;
        public BaseMenuDisplay() : base(DEFAULT_WIDTH, DEFAULT_HEIGHT)
        {
            UseMouse = false;
            Cursor.IsVisible = true;
            Position = DEFAULT_POSITION;
        }
    }
    private class StoryDisplay : BaseMenuDisplay
    {
        public StoryDisplay()
        {
            UseMouse = false;
            Cursor.IsVisible = true;
            Position = new(35, 4);

            string[] text = new string[]
            {
                "[c:r f:ansiwhite]Welcome to the [c:r f:black][c:r b:White]Story mode[c:u]!",
                "",
                "[c:r f:gray]Long string of text so i know it fucking works",
                "Not implemented yet."
            };

            instructions = new(ColoredString.Parser.Parse(string.Join("\r\n", text)));
            instructions.Cursor = this.Cursor;
            instructions.TotalTimeToPrint = TimeSpan.FromSeconds(2);
            instructions.Finished += (s, a) =>
            {
                Cursor.IsVisible = false;
            };

            SadComponents.Add(instructions);

            Border.BorderParameters borderParameters = Border.BorderParameters.GetDefault().AddTitle("Story");
            new Border(this, borderParameters);
        }
    }

    private class ParadoxSimulationDisplay : ControlsConsole
    {
        private DrawString instructions;
        private CharacterMenu characterSelection;
        private Console? c; // inner display

        public ParadoxSimulationDisplay(Action<GameEvent> callback) : base(DEFAULT_WIDTH, DEFAULT_HEIGHT)
        {
            UseMouse = false;
            Cursor.IsVisible = true;
            Position = DEFAULT_POSITION;

            string[] text = new string[]
            {
                "Welcome to the Paradox Simulator",
                "",
                "[c:r f:gray]This section is intended as a challenge and a demonstration of character abilities.",
                "[c:r f:red](As well as debug them)"
            };

            instructions = new(ColoredString.Parser.Parse(string.Join("\r\n", text)));
            instructions.Cursor = this.Cursor;
            instructions.TotalTimeToPrint = TimeSpan.FromSeconds(2);
            instructions.Finished += (s, a) =>
            {
                Cursor.IsVisible = false;
            };

            SadComponents.Add(instructions);

            characterSelection = new();

            Border.BorderParameters borderParameters = Border.BorderParameters.GetDefault().AddTitle("Paradox Simulation");
            new Border(this, borderParameters);

            // Link the character selection event to the event passed into this object.
            // This routes the event back to the main menu for processing.
            characterSelection.Callback += callback;

            // Link the menu with this object so that the menu changes depending on which character is selected
            characterSelection.Callback += Process;

            Render();
        }

        private void Render()
        {
            Children.Clear();
            Children.Add(characterSelection);
            if(c != null)
                Children.Add(c);

            Border.BorderParameters borderParameters = Border.BorderParameters.GetDefault().AddTitle("Paradox Simulation");
            new Border(this, borderParameters);
        }

        private void Process(GameEvent e)
        {
            if(e.EventData.Contains("saki"))
            {
                PrintScenarioDetails("saki");
            }
            else if (e.EventData.Contains("hirina"))
            {
                PrintScenarioDetails("hirina");
            }
            else if (e.EventData.Contains("mariah"))
            {
                PrintScenarioDetails("mariah");
            }
            else if(e.EventData.Contains("luna"))
            {
                PrintScenarioDetails("luna");
            }
            else if (e.EventData.Contains("minako"))
            {
                PrintScenarioDetails("minako");
            }
            else if (e.EventData.Contains("lianna"))
            {
                PrintScenarioDetails("lianna");
            }
        }

        private void PrintScenarioDetails(string character)
        {
            string[] scenarioText;
            c = new(110, 20)
            {
                Position = new(41, 8),
            };

            switch (character)
            {
                case "saki":
                    scenarioText = new string[]
                    {
                        "[c:r f:yellow]Saki[c:u] is a powerful mage but requires long setup to fully utilize her abilities.",
                        "[c:r f:blue]Magic circles[c:u] increase her usable mana allowing more powerful spells.",
                    };
                    break;
                case "hirina":
                    scenarioText = new string[]
                    {
                        "[c:r f:red]Hirina[c:u] is an excellent melee guard with high physical and magic abilities.",
                        "She excels in single target damage but her crowd control abilities are expensive.",
                    };
                    break;
                case "mariah":
                    scenarioText = new string[]
                    {
                        "[c:r f:lightblue]Mariah[c:u] has high movement values, making her suitable for battle openings.",
                        "Combine this with ranged sword attacks to clear fodder enemies early on before they become major threats.",
                    };
                    break;
                case "luna":
                    scenarioText = new string[]
                    {
                        "[c:r f:purple]Luna[c:u] is an engineer and requires some setup like [c:r f:yellow]Saki[c:u]",
                        "[c:r f:brown]Turrets[c:u] are her main source of damage and can be used to clear crowds.",
                        "[c:r f:brown]Turrets[c:u] can also distract enemies, allowing other characters to prepare.",
                        "Alternatively, [c:r f:green]machines[c:u] can be used to support the team with ammunition.",
                    };
                    break;
                case "minako":
                    scenarioText = new string[]
                    {
                        "[c:r f:gray]Minako[c:u] uses long range weapons to take out powerful enemies without a scratch.",
                        "Unfortunately, her melee abilities are lacking and she is vulnerable to close range attacks.",
                        "Her weapons also use ammunition. [c:r f:purple]Luna[c:u]'s machines can replenish this supply.",
                        "Reloading takes time, so use other characters to distract enemies while she reloads.",
                    };
                    break;
                case "lianna":
                    scenarioText = new string[]
                    {
                        "[c:r f:yellow]Lianna[c:u] is versatile support unit able to switch between healing and damage.",
                        "As a brewer, she can concoct elixirs that can damage and debuff enemies.",
                        "Producing complex potions takes time and a cauldron, so think and prepare carefully.",
                        "",
                        "If potions are not your style, she can also use her [c:r f:blue]magic[c:u] to buff and heal the team."
                    };
                    break;
                default:
                    scenarioText = new string[] { "" };
                    break;
            }

            instructions = new(ColoredString.Parser.Parse(string.Join("\r\n", scenarioText)));
            instructions.Cursor = c.Cursor;

            c.SadComponents.Add(instructions);

            Render();
        }

        private class CharacterMenu : BaseMenu
        {
            private Dictionary<string, GameEvent> options;
            private const int wide = 15;
            public CharacterMenu() : base(wide, 20, "Characters")
            {
                options = new()
                {
                    { "Mariah", new(EventType.IDC, new("mariah")) },
                    { "Saki",   new(EventType.IDC, new("saki")) },
                    { "Hirina", new(EventType.IDC, new("hirina")) },
                    { "Luna",   new(EventType.IDC, new("luna")) },
                    { "Minako", new(EventType.IDC, new("minako")) },
                    { "Lianna", new(EventType.IDC, new("lianna")) }
                };

                Position = new(0, 7);
                Render();
            }
            public override void Render()
            {
                ListBox lb = new(wide, options.Count + 2)
                {
                    DrawBorder = true,
                    BorderLineStyle = (int[])ICellSurface.ConnectedLineThick.Clone(),
                    Position = new(0, 0)
                };

                foreach(string option in options.Keys)
                {
                    lb.Items.Add(option);
                }

                lb.SelectedItemChanged += (e, a) =>
                {
                    OnCallback(options[lb.SelectedItem.ToString()]);
                };

                Controls.Add(lb);
            }
        }
    }

    private class ArenaDisplay : ControlsConsole
    {
        public ArenaDisplay(Action<GameEvent> callback) : base(DEFAULT_WIDTH, DEFAULT_HEIGHT)
        {
            Position = DEFAULT_POSITION;

            Button b = new(10);
            b.Text = "Start";
            b.Click += (e, a) =>
            {
                GameEvent @event = new(EventType.CHANGE_SCENE, new("arena"));
                callback.Invoke(@event);
            };

            Controls.Add(b);
        }
    }
}
