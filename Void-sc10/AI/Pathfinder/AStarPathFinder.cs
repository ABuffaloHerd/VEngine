using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Scenes.Combat;

namespace VEngine.AI.Pathfinder
{
    public class AStarPathFinder
    {
        private Arena arena;

        public AStarPathFinder(Arena arena)
        {
            this.arena = arena;
        }

        public Point? FindEndpoint(Point start, Point end, int maxSteps)
        {
            var openList = new List<Point>();
            var closedList = new HashSet<Point>();
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
            Dictionary<Point, int> gScore = new Dictionary<Point, int>() { [start] = 0 };
            Dictionary<Point, int> fScore = new Dictionary<Point, int>() { [start] = Heuristic(start, end) };

            openList.Add(start);

            while (openList.Count > 0)
            {
                var current = GetLowestFScore(openList, fScore);
                int currentSteps = gScore[current];

                if (current == end || currentSteps >= maxSteps)
                    return current; // Return the current point when steps limit is reached or the end is found

                openList.Remove(current);
                closedList.Add(current);

                foreach (var neighbor in GetNeighbors(current))
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
            }

            return null; // No path found within the steps limit
        }

        private int Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y); // Manhattan distance
        }

        private List<Point> GetNeighbors(Point p)
        {
            var neighbors = new List<Point>
        {
            new Point(p.X + 1, p.Y), new Point(p.X - 1, p.Y),
            new Point(p.X, p.Y + 1), new Point(p.X, p.Y - 1)
        };
            return neighbors;
        }

        private Point GetLowestFScore(List<Point> openList, Dictionary<Point, int> fScore)
        {
            Point lowest = openList[0];
            foreach (var point in openList)
                if (fScore[point] < fScore[lowest])
                    lowest = point;
            return lowest;
        }
    }
}

