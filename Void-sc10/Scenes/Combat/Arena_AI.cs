using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Objects;

namespace VEngine.Scenes.Combat
{
    public partial class Arena
    {
        public Graph ToGraph()
        {
            var graph = new Graph();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var point = new Point(x, y);
                    if (At(point) is not StaticGameObject)
                    {
                        graph.AddNode(point);

                        var neighbors = new List<Point>
                        {
                            new Point(x + 1, y), new Point(x - 1, y),
                            new Point(x, y + 1), new Point(x, y - 1)
                        };

                        foreach (var neighbor in neighbors)
                        {
                            if (IsWithinBounds(neighbor) && IsTileFree(neighbor))
                            {
                                graph.AddNode(neighbor);
                                graph.AddEdge(point, neighbor);
                                graph.AddEdge(neighbor, point); // Add bidirectional edge
                            }
                        }
                    }
                }
            }

            return graph;
        }
    }
}
