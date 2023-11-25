using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Objects
{
    public class PlayerGameObject : GameObject, IControllable
    {
        public PlayerGameObject(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {

        }

        public List<ControlBase> GetControls()
        {
            throw new NotImplementedException();
        }
    }
}
