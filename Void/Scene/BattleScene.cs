using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Scene
{
    // generic battle scene
    // contains the map object
    public class BattleScene : BaseScene
    {
        public BattleScene() : base()
        {
            gmInstance = GameManager.Instance;


        }
        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
