using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Scenes.Combat;
using VEngine.Logging;
using System.Runtime.CompilerServices;
using VEngine.Data;

namespace VEngine.AI.Pathfinder
{
    public static class DijkstraPathFinder
    {
        public static List<Point> FindPath(Graph graph, Point start, Point end)
        {
            if (!graph.AdjacencyList.ContainsKey(start) || !graph.AdjacencyList.ContainsKey(end))
            {
                return new List<Point>(); // Return an empty path if start or end is not in the graph
            }

            var openSet = new PriorityQueue<Point, int>();
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, int> { [start] = 0 };

            openSet.Enqueue(start, 0);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                if (current == end)
                    return ReconstructPath(cameFrom, current);

                if (!graph.AdjacencyList.ContainsKey(current))
                {
                    Logger.Report(current, $"Error: Current point {current} not found in adjacency list");
                    continue;
                }

                foreach (var neighbor in graph.AdjacencyList[current])
                {
                    int tentativeGScore = gScore[current] + 1;

                    if (tentativeGScore < gScore.GetValueOrDefault(neighbor, int.MaxValue))
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        openSet.Enqueue(neighbor, tentativeGScore);
                    }
                }

                // Debugging logs
                Logger.Report(current, $"Current: {current} | gScore: {gScore[current]}");
                Logger.Report(openSet.Count, $"Open Set Size: {openSet.Count}");
            }

            Logger.Report(null, "No path found");
            return new List<Point>(); // Return an empty path if no path is found
        }

        private static List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            var totalPath = new List<Point> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Insert(0, current);
            }
            return totalPath;
        }
    }

}
