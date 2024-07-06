using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.AI.Pathfinder
{
    public class ClosestPointFinder
    {
        private static readonly int[] dx = { 1, -1, 0, 0 };
        private static readonly int[] dy = { 0, 0, 1, -1 };

        public static Point FindClosestPoint(Point center, int radius, Point target)
        {
            Queue<Point> queue = new();
            HashSet<(int, int)> visited = new();

            queue.Enqueue(center);
            visited.Add((center.X, center.Y));

            Point closestPoint = (-1, -1);
            double minDistance = double.MaxValue;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                double currentDistance = GetDistance(current, target);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closestPoint = current;
                }

                for (int i = 0; i < 4; i++)
                {
                    int newX = current.X + dx[i];
                    int newY = current.Y + dy[i];

                    if (Math.Abs(newX - center.X) + Math.Abs(newY - center.Y) <= radius && !visited.Contains((newX, newY)))
                    {
                        visited.Add((newX, newY));
                        queue.Enqueue(new Point(newX, newY));
                    }
                }
            }

            return closestPoint;
        }

        private static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}
