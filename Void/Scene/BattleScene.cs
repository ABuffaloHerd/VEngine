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
            Children.Add(arena);
        }
        public override void Render()
        {
            throw new NotImplementedException();
        }

        private void Process(GameEvent e)
        {
            Pattern p = new();
            p.Mark(0, 0);
            p.Mark(1, 0);
            p.Mark(2, 0);
            p.Mark(3, 0);

            p.Mark(1, -1);
            p.Mark(1, 1);

            arena.Mark(p, new(5, 5));
        }

        private class BattleControls : BaseMenu
        {
            public BattleControls(int width, int height) : base(width, height)
            {
                Button b = new(20)
                {
                    Text = "Button",
                    Position = new(0, 0),
                };

                b.Click += (s, a) =>
                {
                    OnCallback(new(EventType.IDC, new("button")));
                };

                Controls.Add(b);
            }

            public override void Render()
            {
                throw new NotImplementedException();
            }
     }
    }
}
