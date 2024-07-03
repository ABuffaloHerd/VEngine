using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public static class DirectionExtensions
    {
        public static Point ToVector(this Direction direction)
        {
            return direction switch
            {
                Direction.UP => (0, -1),
                Direction.DOWN => (0, 1),
                Direction.LEFT => (-1, 0),
                Direction.RIGHT => (1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}")
            };
        }

        public static Direction FromVector(this Point p) // extends the point class
        {
            return p switch
            {
                { X: 0, Y: -1 } => Direction.UP,
                { X: 0, Y: 1 } => Direction.DOWN,
                { X: -1, Y: 0 } => Direction.LEFT,
                { X: 1, Y: 0 } => Direction.RIGHT,
                _ => throw new ArgumentOutOfRangeException(nameof(p), $"Not expected point value: {p}")
            };
        }

    }
}
