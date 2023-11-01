using SadConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Battle
{
    public interface IControllable
    {
        ControlsConsole GetControls();
    }

    public interface IMovable
    {
        void Move(Point delta);
        void Backdash(Point delta);
    }
}
