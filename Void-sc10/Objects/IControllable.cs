using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Objects
{
    public interface IControllable
    {
        /// <summary>
        /// Get this object's controls to be added to the scene's controls
        /// </summary>
        /// <returns></returns>
        List<ControlBase> GetControls();
    }
}
