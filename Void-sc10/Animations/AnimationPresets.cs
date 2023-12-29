using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Animations
{
    public static class AnimationPresets
    {
        public static Point FontSize { get; set; }
        static AnimationPresets()
        {

        }

        public static AnimatedEffect ExplosionEffect(int radius, TimeSpan speed)
        {
            // makes the size of the explosion always odd so that it's centered
            AnimatedEffect ef = new("explosion", (radius * 2) - 1, (radius * 2) - 1, TimeSpan.Zero, (0, 0));
            ef.FontSize = FontSize;
            ef.Repeat = false; // stop after playing once
            ef.AnimationDuration = speed;

            Random rand = new();

            char blast = '#';
            char[] particles = { '*', '+', '.', '^' };

            // setup frames
            // we'll use the radius for number of frames.
            // fc = frame count
            // r = current radius

            radius--; // decrement radius because everything after needs -1 and that's ugly.
            int r = 0;
            for(int fc = 0; fc <= radius; fc++)
            {
                ef.CreateFrame();

                // now for each point in the circumference, we set the char to blast.
                foreach(var point in GetPointsInCircumference(++r, (radius, radius))) 
                {
                    ef.Frames[fc].SetGlyph(point.X, point.Y, blast);
                }

                // then put random particles inside the circle.
                // the formula is ceil(2 ^ (x / 1.6) - 2)
                int numberOfPoints = (int)Math.Ceiling(Math.Pow(2, radius/1.6f) - 2); 
                var innerPoints = GetRandomPointsInCircle(r - 2, (radius, radius), numberOfPoints);
                foreach (var point in innerPoints)
                {
                    char particle = particles[rand.Next(particles.Length)];
                    ef.Frames[fc].SetGlyph(point.X, point.Y, particle);
                    ef.Frames[fc].SetForeground(point.X, point.Y, Color.Gray);
                }
            }

            // lastly, add an empty frame.
            ef.CreateFrame();

            return ef;
        }

        private static HashSet<Point> GetPointsInCircumference(int radius, Point center)
        {
            HashSet<Point> points = new();

            for (int theta = 0; theta < 360; theta++)
            {
                double rads = theta * Math.PI / 180;
                int x = center.X + (int)(radius * Math.Cos(rads));
                int y = center.Y + (int)(radius * Math.Sin(rads));

                points.Add((x, y));
            }

            return points;
        }

        private static HashSet<Point> GetRandomPointsInCircle(int radius, Point center, int numberOfPoints)
        {
            HashSet<Point> points = new HashSet<Point>();
            Random rnd = new Random();
            int centerX = center.X;
            int centerY = center.Y;
            int radiusSquared = radius * radius;

            for (int count = 0; count < numberOfPoints; count++)
            {
                // Generate a random point within the bounding box
                int x = rnd.Next(centerX + radius);
                int y = rnd.Next(centerY + radius);

                // Check if the point is within the circle
                if ((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY) <= radiusSquared)
                {
                    points.Add(new Point(x, y));
                }
            }

            return points;
        }
    }
}
