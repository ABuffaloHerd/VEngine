using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Logging;
using VEngine.Scenes.Combat;

namespace VEngine.AI.Pathfinder
{
    public static class AStarPathFinder
    {
        public static Point? FindEndpoint(Arena arena, Point start, Point end, int maxSteps)
        {
            var openList = new List<Point>();
            var closedList = new HashSet<Point>();
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, int> { [start] = 0 };
            var fScore = new Dictionary<Point, int> { [start] = Heuristic(start, end) };

            openList.Add(start);

            Point? closestPoint = null;
            int closestDistance = int.MaxValue;

            while (openList.Count > 0)
            {
                var current = GetLowestFScore(openList, fScore);
                if (current == end)
                    return current; // Return the end point if found

                openList.Remove(current);
                closedList.Add(current);

                if (gScore[current] >= maxSteps)
                    continue; // Skip processing if maxSteps is reached

                int distanceToEnd = Heuristic(current, end);
                if (distanceToEnd < closestDistance)
                {
                    closestDistance = distanceToEnd;
                    closestPoint = current;
                }

                foreach (var neighbor in GetNeighbors(current, arena))
                {
                    if (closedList.Contains(neighbor) || !arena.IsTileFree(neighbor))
                        continue;

                    int tentativeGScore = gScore[current] + 1; // Assuming uniform cost for simplicity

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                    else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, int.MaxValue))
                        continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, end);
                }

                // Debugging logs
                Logger.Report(current, $"Current: {current} | gScore: {gScore[current]} | fScore: {fScore[current]}");
                Logger.Report(openList.Count, $"Open List Size: {openList.Count}");
                Logger.Report(closedList.Count, $"Closed List Size: {closedList.Count}");
            }

            Logger.Report(closestPoint, $"Closest point found: start:{start} end:{end}");
            return closestPoint; // Return the closest point found within the steps limit
        }

        private static int Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y); // Manhattan distance
        }

        private static List<Point> GetNeighbors(Point p, Arena arena)
        {
            var neighbors = new List<Point>();
            var potentialNeighbors = new List<Point>
        {
            new Point(p.X + 1, p.Y), new Point(p.X - 1, p.Y),
            new Point(p.X, p.Y + 1), new Point(p.X, p.Y - 1)
        };

            foreach (var neighbor in potentialNeighbors)
            {
                if (arena.IsWithinBounds(neighbor) && arena.IsTileFree(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private static Point GetLowestFScore(List<Point> openList, Dictionary<Point, int> fScore)
        {
            Point lowest = openList[0];
            foreach (var point in openList)
                if (fScore[point] < fScore[lowest])
                    lowest = point;
            return lowest;
        }
    }

}

