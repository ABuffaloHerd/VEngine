using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Data
{
    public class Pattern : IEnumerable<Point>
    {
        private HashSet<Point> points;

        public IEnumerable<Point> Points { get => points; } 

        /// <summary>
        /// <inheritdoc cref="Pattern.Pattern()"/>
        /// </summary>
        /// <param name="points">Points to use</param>
        public Pattern(HashSet<Point> points)
        {
            this.points = points;
        }

        /// <summary>
        /// Patterns are always defined facing right.
        /// </summary>
        public Pattern()
        {
            points = new();
        }

        public Pattern Mark(Point p)
        {
            points.Add(p);

            return this;
        }

        public Pattern Mark(int x, int y)
        {
            points.Add(new(x, y));

            return this;
        }

        public void Mark(ICollection<Point> points)
        {
            foreach(var point in points)
            {
                this.points.Add(point);
            }
        }

        /// <summary>
        /// Returns a collection of points that have been rotated without modifying the actual data
        /// This is all rotated relative to 0, 0
        /// </summary>
        /// <param name="angle">angle to rotate by. Accepts 90, 180 and 270</param>
        /// <returns></returns>
        public ICollection<Point> GetRotated(int angle)
        {
            HashSet<Point> newPoints = new();

            foreach(var point in points)
            {
                newPoints.Add(Rotate(point, angle));
            }

            return newPoints;
        }

        private Point Rotate(Point point, int angle)
        {
            angle = angle % 360;
            if (angle < 0)
                angle += 360;

            int newX, newY;

            switch (angle)
            {
                case 90:
                    // 90 degrees clockwise rotation
                    newX = -point.Y;
                    newY = point.X;
                    break;
                case 180:
                    // 180 degrees rotation
                    newX = -point.X;
                    newY = -point.Y;
                    break;
                case 270:
                    // 270 degrees clockwise (or 90 degrees counterclockwise) rotation
                    newX = point.Y;
                    newY = -point.X;
                    break;
                default:
                    // No rotation or invalid rotation angle
                    return point;
            }

            return new Point(newX, newY);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
