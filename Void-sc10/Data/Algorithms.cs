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
            double dist = double.MaxValue;
            GameObject? closest = null;

            foreach (var obj in objects)
            {
                double mag = Point.EuclideanDistanceMagnitude(obj.Position, origin);

                if (mag < dist)
                {
                    dist = mag;
                    closest = obj;
                }
            }

            return closest;
        }

        /// <summary>
        /// Returns a list of points that make up a circle. All integers.
        /// </summary>
        /// <param name="radius">Radius of the circle</param>
        /// <param name="center">Center of the circle</param>
        /// <returns></returns>
        public static HashSet<Point> GetPointsInCircumference(int radius, Point center)
        {
            HashSet<Point> points = new();

            for (int theta = 0; theta < 360; theta++)
            {
                double rads = theta * Math.PI / 180;
                int x = center.X + (int)(radius * Math.Cos(rads));
                int y = center.Y + (int)(radius * Math.Sin(rads));

                points.Add((x, y));
            }

            return points;
        }

        /// <summary>
        /// Returns a random list of points within a specified radius.
        /// </summary>
        /// <param name="radius">Radius to check</param>
        /// <param name="center">Center of the circle</param>
        /// <param name="numberOfPoints">Number of times to try generating a valid point</param>
        /// <returns></returns>
        public static HashSet<Point> GetRandomPointsInCircle(int radius, Point center, int numberOfPoints)
        {
            HashSet<Point> points = new HashSet<Point>();
            Random rnd = new Random();
            int centerX = center.X;
            int centerY = center.Y;
            int radiusSquared = radius * radius;

            for (int count = 0; count < numberOfPoints; count++)
            {
                // Generate a random point within the bounding box
                int x = rnd.Next(centerX + radius);
                int y = rnd.Next(centerY + radius);

                // Check if the point is within the circle
                if ((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY) <= radiusSquared)
                {
                    points.Add(new Point(x, y));
                }
                // else it is discarded
            }

            return points;
        }
    }
}
