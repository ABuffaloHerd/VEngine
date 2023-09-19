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
                //System.Console.WriteLine("Arena move");
                System.Console.WriteLine(e.EventData.Get("move"));
            }

            if (e.EventType == EventType.IDC)
            {
                if (e.EventData.Contains("range"))
                {
                    arena.Mark();
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
                        new("move", new Point(1, 1))
                        )
                    );
                };

                Controls.Add(b);
                Controls.Add(moveButton);
            }

            public override void Render()
            {
                throw new NotImplementedException();
            }
        }
    }
}
