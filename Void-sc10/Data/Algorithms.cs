using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Logging;
using VEngine.Objects;

namespace VEngine.Data
{
    public static class Algorithms
    {
        /// <summary>
        /// finds and returns the closest game object
        /// </summary>
        /// <param name="objects">List of objects in a valid range</param>
        /// <param name="origin">Position</param>
        /// <returns>null if list is empty</returns>
        public static GameObject? Closest(IEnumerable<GameObject> objects, Point origin)
        {
            Logger.Report("Algorithms.Closest", "closest running checks");
            double dist = double.MaxValue;
            GameObject closest = null;

            foreach (var obj in objects)
            {
                Logger.Report("GameObject.Closest", $"checking object {obj}, position is {obj.Position}");
                double mag = Point.EuclideanDistanceMagnitude(obj.Position, origin);

                if (mag < dist)
                {
                    dist = mag;
                    closest = obj;
                }
            }

            return closest;
        }
    }
}
