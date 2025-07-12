using System;
using System.Collections.Generic;
using VEngine.Logging;
using VEngine.Scenes.Combat;

namespace VEngine.AI.Pathfinder
{
    public static class AStarPathFinder
    {
        private const int MOVEMENT_COST = 1;

        public static Point? FindEndpoint(Arena arena, Point start, Point end, int maxSteps)
        {
            // Parameter validation
            if (arena == null)
                throw new ArgumentNullException(nameof(arena));
            
            if (maxSteps <= 0)
                throw new ArgumentException("maxSteps must be positive", nameof(maxSteps));

            if (!arena.IsWithinBounds(start) || !arena.IsWithinBounds(end))
                return null;

            if (!arena.IsTileFree(start) || !arena.IsTileFree(end))
                return null;

            // Use PriorityQueue for better performance
            var openList = new PriorityQueue<Point, int>();
            var closedList = new HashSet<Point>();
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, int> { [start] = 0 };
            var fScore = new Dictionary<Point, int> { [start] = Heuristic(start, end) };

            openList.Enqueue(start, fScore[start]);

            Point? closestPoint = null;
            int closestDistance = int.MaxValue;

            while (openList.Count > 0)
            {
                var current = openList.Dequeue();
                
                if (current == end)
                    return current; // Return the end point if found

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
                    if (closedList.Contains(neighbor))
                        continue;

                    int tentativeGScore = gScore[current] + MOVEMENT_COST;

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        int newFScore = gScore[neighbor] + Heuristic(neighbor, end);
                        fScore[neighbor] = newFScore;
                        
                        // Re-add to priority queue with new priority
                        openList.Enqueue(neighbor, newFScore);
                    }
                }
            }

            return closestPoint; // Return the closest point found within the steps limit
        }

        private static int Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y); // Manhattan distance
        }

        private static List<Point> GetNeighbors(Point p, Arena arena)
        {
            var neighbors = new List<Point>(4); // Pre-allocate capacity for 4 neighbors
            var potentialNeighbors = new[]
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
    }
}

