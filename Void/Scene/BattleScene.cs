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
    public class BattleScene : Scene
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
            GameObject t = new TestObject('T').SetPosition(6, 6);
            t.Name = "Steven";
            arena.Add(t);

            GameObject S = new TestObject('S').SetPosition(5, 6);
            S.Name = "Frank";
            arena.Add(S);

            arena.Add(new TestObject('E').SetPosition(3, 4));

            Children.Add(arena);
        }
        public override void Render()
        {
            throw new NotImplementedException();
        }

        protected override void Process(IGameEvent e)
        {
            // When a new event is fired, tell the arena to remove all non-entity renders
            arena.Clear();
            arena.ResetObjects();

            if (e.EventType == EventType.ARENA_MOVE)
            {
                arena.Move(e.GetData<Point>("move"));
            }

            if (e.EventType == EventType.IDC)
            {
                if (e.ContainsValue("range"))
                {
                    arena.Mark();
                }

                if(e.EventData.ContainsValue("rangecheck"))
                {
                    var a = arena.InRange();

                    foreach(GameObject gameobj in a)
                    {
                        System.Console.WriteLine(gameobj.ToString());
                    }
                }
            }
        }

        private class BattleControls : BaseMenu
        {
            public BattleControls(int width, int height) : base(width, height)
            {
                this.Surface.DefaultBackground = Color.White;

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
