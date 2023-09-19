using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.DataStructures
{
    // struct that has a list of marked tiles
    public sealed class Pattern : IEnumerable<Point>
    {
        public List<Point> Points { get; init; }

        public Pattern(List<Point> points)
        {
            Points = points;
        }

        public Pattern()
        {
            Points = new();
        }

        public void Mark(Point p)
        {
            Points.Add(p);
        }

        public void Mark(int x, int y)
        {
            Points.Add(new(x, y));
        }

        public void Mark(Pattern r)
        {
            Points.AddRange(r.Points);
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            for(int y = 0; y <= GetRange().Item2; y++)
            {
                for(int x = 0; x <= GetRange().Item1; x++)
                {
                    if(Points.Contains(new(x, y)))
                    {
                        sb.Append("X");
                    }
                    else
                    {
                        sb.Append(" ");
                    }
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

        private void Sort()
        {
            Points.Sort((a, b) => a.X.CompareTo(b.X));
            Points.Sort((a, b) => a.Y.CompareTo(b.Y));
        }

        private Tuple<int, int> GetRange()
        {
            int maxX = 0, maxY = 0;
            Sort();

            foreach(Point point in Points) 
            {
                if(point.X > maxX)
                {
                    maxX = point.X;
                }

                if(point.Y > maxY)
                {
                    maxY = point.Y;
                }
            }

            return new(maxX, maxY);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return Points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Points.GetEnumerator();
        }
    }
}
