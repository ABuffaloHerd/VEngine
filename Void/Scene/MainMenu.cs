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

public partial class MainMenu : BaseScene
{
    private ListBoxMenu menu;
    private Console title;
    private Console display;
    public MainMenu() : base()
    {
        SceneType = SceneType.MENU;

        Dictionary<string, GameEvent> options = new()
        {
            { "Story",              new(EventType.IDC, new("story")) },
            { "Paradox Simulation", new(EventType.IDC, new("paradox")) },
            { "Arena",              new(EventType.IDC, new("arena")) },
            { "Battle test",        new(EventType.IDC, new("test_battle")) },
            { "Back",               new(EventType.CHANGE_MENU,  new("title")) }
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

    protected override void Process(GameEvent e)
    {
        System.Console.WriteLine("Main menu scene has received a game event");
        System.Console.WriteLine(e.ToString());

        // Handle switching back to the title by offloading it to the game manager.
        if (e.EventType == EventType.CHANGE_MENU)
        {
            if (e.EventData.Contains("title"))
            {
                gmInstance.Raise(new(EventType.CHANGE_MENU, new("title")));
            }
        }

        if (e.EventData.Contains("story"))
        {
            display = new StoryDisplay();
        }
        else if (e.EventData.Contains("paradox"))
        {
            display = new ParadoxSimulationDisplay(Process);
        }
        else if (e.EventData.Contains("arena"))
        {
            if (e.EventType == EventType.IDC)
                display = new ArenaDisplay(Process);
            else
                gmInstance.Raise(e); // send it back to the game manager.
        }

        Render();
    }
}
