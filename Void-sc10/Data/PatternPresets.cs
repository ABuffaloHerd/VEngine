using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Data
{
    public partial class Pattern
    {
        public static class Presets
        {
            public static List<Point> Diamond(int radius)
            {
                var points = new List<Point>();

                // The center of the diamond will be at (0, 0)
                Point center = new Point(0, 0);
                int diameter = radius * 2;

                for (int y = -radius; y <= radius; y++)
                {
                    // Determine the width of the diamond at the current y-level.
                    // The diamond's width increases then decreases as you move along the y-axis.
                    int width = radius - Math.Abs(y);

                    for (int x = -width; x <= width; x++)
                    {
                        points.Add(new Point(center.X + x, center.Y + y));
                    }
                }

                return points;
            }
        }
    }
}
