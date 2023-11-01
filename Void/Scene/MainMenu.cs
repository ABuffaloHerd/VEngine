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

public partial class MainMenu : Scene
{
    private ListBoxMenu menu;
    private Console title;
    private Console display;
    public MainMenu() : base()
    {
        SceneType = SceneType.MENU;

        Dictionary<string, IGameEvent> options = new()
        {
            { "Story",              new SceneEvent("story") },
            { "Paradox Simulation", new SceneEvent("paradox") },
            { "Arena",              new SceneEvent("arena") },
            { "Battle test",        new SceneEvent("test_battle") },
            { "Back",               new SceneEvent("title") }
        };

        menu = new(40, 40, options)
        {
            Position = new(0, 3)
        };

        title = new(10, 1)
        {
            FontSize = new(15, 25),
            Position = new(0, 0),
            UseMouse = false
        };

        title.Print(0, 0, "Main Menu");

        display = new(10, 10); // new blank console as placeholder
        display.Position = new(40, 0);

        // subscribe to child menu's callback so that we can recieve events.
        menu.Callback += Process;

        Render();
    }

    public override void Render()
    {
        Children.Clear();
        Children.Add(menu);
        Children.Add(title);
        Children.Add(display);
    }

    protected override void Process(IGameEvent e)
    {
        System.Console.WriteLine("Main menu scene has received a game event");
        System.Console.WriteLine(e.ToString());

        // Handle switching back to the title by offloading it to the game manager.
        if (e.EventType == EventType.CHANGE_MENU)
        {
            if (e.Contains("title"))
            {
                GameEvent @event = new GameEvent(EventType.CHANGE_MENU);
                gmInstance.Raise(@event);
            }
        }

        if (e.Contains("story"))
        {
            display = new StoryDisplay();
        }
        else if (e.Contains("paradox"))
        {
            display = new ParadoxSimulationDisplay(Process);
        }
        else if (e.Contains("arena"))
        {
            if (e.EventType == EventType.IDC)
                display = new ArenaDisplay(Process);
            else
                gmInstance.Raise(e); // send it back to the game manager.
        }

        Render();
    }
}
