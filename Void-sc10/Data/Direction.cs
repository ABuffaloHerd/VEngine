using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Data
{
    /// <summary>
    /// Directions and their rotations from right facing in degrees
    /// </summary>
    public enum Direction
    {
        UP = 270,
        DOWN = 90,
        LEFT = 180,
        RIGHT = 0
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
