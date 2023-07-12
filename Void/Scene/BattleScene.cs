using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Battle;

namespace Void.Scene
{
    // generic battle scene
    // contains the map object
    public class BattleScene : BaseScene
    {
        public BattleScene() : base()
        {
            gmInstance = GameManager.Instance;
            System.Console.WriteLine("Battlescene constructor works");
            Children.Add(new Arena(90, 50, Color.Chocolate));
        }
        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
