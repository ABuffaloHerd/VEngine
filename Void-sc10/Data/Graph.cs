using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Scenes.Combat;

namespace VEngine.Data
{
    public class Graph
    {
        public Dictionary<Point, List<Point>> AdjacencyList { get; } = new();

        public void AddNode(Point point)
        {
            if (!AdjacencyList.ContainsKey(point))
            {
                AdjacencyList[point] = new List<Point>();
            }
        }

        public void AddEdge(Point from, Point to)
        {
            if (AdjacencyList.ContainsKey(from) && AdjacencyList.ContainsKey(to))
            {
                AdjacencyList[from].Add(to);
            }
        }
    }

}
