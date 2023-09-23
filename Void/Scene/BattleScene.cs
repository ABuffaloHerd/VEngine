using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Battle;
using Void.DataStructures;
using Void.Event;
using Void.UI;

namespace Void.Scene
{
    // generic battle scene
    // contains the map object
    public class BattleScene : BaseScene
    {
        private BattleControls controls;
        private Arena arena;
        public BattleScene() : base()
        {
            controls = new(70, 10)
            {
                Position = new(0, 51)
            };

            controls.Callback += Process;
            Children.Add(controls);
            
            arena = new Arena(90, 50, Color.Black);
            arena.Add(new TestObject('T'));
            arena.Add(new TestObject('E').SetPosition(3, 4));

            Children.Add(arena);

            arena.Update();
        }
        public override void Render()
        {
            throw new NotImplementedException();
        }

        private void Process(GameEvent e)
        {
            arena.Clear();
            if (e.EventType == EventType.ARENA_MOVE)
            {
                arena.Move(arena.Selected.ID, (Point)e.EventData.Get("move"));
                System.Console.WriteLine(e.EventData.Get("move"));
            }

            if (e.EventType == EventType.IDC)
            {
                if (e.EventData.Contains("range"))
                {
                    arena.Mark();
                }

                if(e.EventData.Contains("rangecheck"))
                {
                    var a = arena.InRange();

                    foreach(GameObject gameobj in a)
                    {
                        System.Console.WriteLine(a.ToString());
                        System.Console.WriteLine("haha jonathan the tostring is broken");
                    }
                }
            }
        }

        private class BattleControls : BaseMenu
        {
            public BattleControls(int width, int height) : base(width, height)
            {
                Button b = new(20)
                {
                    Text = "Range",
                    Position = new(0, 0),
                };

                b.Click += (s, a) =>
                {
                    OnCallback(new(EventType.IDC, new("range")));
                };

                Button moveButton = new(20)
                {
                    Text = "Move",
                    Position = new(0, 2)
                };

                moveButton.Click += (s, a) =>
                {
                    OnCallback(
                        new(EventType.ARENA_MOVE, 
                        new("move", new Point(0, 1))
                        )
                    );
                };

                Button rangecheck = new(20)
                {
                    Text = "Range",
                    Position = new(0, 3)
                };
                rangecheck.Click += (s, a) =>
                {
                    OnCallback(
                        new(EventType.IDC,
                        new("rangecheck"))
                        );

                    System.Console.WriteLine("Battle scene rangecheck fired");
                };

                Controls.Add(b);
                Controls.Add(moveButton);
                Controls.Add(rangecheck);
            }

            public override void Render()
            {
                throw new NotImplementedException();
            }
        }
    }
}
