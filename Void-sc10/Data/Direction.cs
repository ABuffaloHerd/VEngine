using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Data
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public enum Rotation
    {
        /// <summary>
        /// clockwise
        /// </summary>
        CW = 0,

        /// <summary>
        /// counter clockwise
        /// </summary>
        CCW = 1,
    }
}
