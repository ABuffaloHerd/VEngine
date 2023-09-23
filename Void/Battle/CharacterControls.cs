using SadConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Event;

namespace Void.Battle
{
    public class CharacterControls : ControlsConsole
    {
        public Action<GameEvent> Callback;
        public CharacterControls(int width, int height) : base(width, height)
        {
        }
    }
}
