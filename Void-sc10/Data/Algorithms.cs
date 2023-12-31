using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Logging;
using VEngine.Objects;
using VEngine.Scenes.Combat;

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

        /// <summary>
        /// Uses Bresenham's line algorithm to determine if there is line of sight between the two points
        /// </summary>
        /// <param name="start">start</param>
        /// <param name="end">end</param>
        /// <param name="isFree">Lambda to check if this tile is free</param>
        /// <returns></returns>
        public static bool HasLineOfSight(Point start, Point end, Func<Point, bool> isFree)
        {
            int x = start.X;
            int y = start.Y;
            int dx = Math.Abs(end.X - start.X);
            int dy = Math.Abs(end.Y - start.Y);
            int sx = start.X < end.X ? 1 : -1;
            int sy = start.Y < end.Y ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                Logger.Report("Algorithms", $"Checking {x}, {y}");
                if (!isFree((x, y)))
                {
                    Logger.Report("Algorithms", "isFree check failed.");
                    return false; // Obstacle found, line of sight is blocked
                }

                if (x == end.X && y == end.Y)
                {
                    break; // Reached end point, no obstacles found
                }

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
                }
            }

            return true; // Clear line of sight
        }

        /// <summary>
        /// Line of sight checking for weapons and spells
        /// </summary>
        /// <param name="targetPosition">target location</param>
        /// <param name="sourcePosition">start location</param>
        /// <param name="target">kill that</param>
        /// <param name="source">I AM NOT A FUCKING OBSTACLE</param>
        /// <param name="arena">arena</param>
        /// <returns>is there a line of sight?</returns>
        public static bool CheckLineOfSight(Point targetPosition, Point sourcePosition, GameObject target, GameObject source, Arena arena)
        {
            Func<Point, bool> isFree = (pos) =>
            {
                var objectAtPos = arena.At(pos.X, pos.Y);
                if (objectAtPos != null)
                {
                    if (objectAtPos.GetHashCode() == target.GetHashCode() || objectAtPos.GetHashCode() == source.GetHashCode())
                    {
                        // It's the target or the source, so not an obstacle
                        return true;
                    }
                    else
                    {
                        // It's some other object, so it's an obstacle
                        return false;
                    }
                }
                else
                {
                    // No object, check if the tile is free
                    return arena.IsTileFree(pos);
                }
            };

            return Algorithms.HasLineOfSight(targetPosition, sourcePosition, isFree);
        }
    }
}
