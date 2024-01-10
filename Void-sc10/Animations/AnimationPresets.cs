using SadConsole.Effects;
using SadRogue.Primitives.SerializedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using VEngine.Data;
using VEngine.Logging;
using Algorithms = VEngine.Data.Algorithms;

namespace VEngine.Animations
{
    /// <summary>
    /// The only reason effects exist in the first place is because of [Haisen "Burari Haieki Gesha no Tabi"]
    /// </summary>
    public static class AnimationPresets
    {
        public static Point FontSize { get; set; }
        static AnimationPresets()
        {

        }

        /// <summary>
        /// Prepares a preset animated screen object for 1x1 character blinking.
        /// </summary>
        /// <param name="glyph">Glyph of character</param>
        /// <param name="fg">foreground colour</param>
        /// <param name="bg">background colour</param>
        /// <returns></returns>
        public static AnimatedScreenObject BlinkingEffect(string name, char glyph, Color fg, Color bg, Point size)
        {
            AnimatedScreenObject aso = new(name, size.X, size.Y)
            {
                AnimationDuration = TimeSpan.FromSeconds(2),
                Repeat = false
            };

            for (int fc = 0; fc < 6; fc++)
            {
                if (fc % 2 == 0)
                {
                    aso.CreateFrame().SetGlyph(0, 0, glyph);
                    aso.Frames[fc].SetForeground(0, 0, fg);
                    aso.Frames[fc].SetBackground(0, 0, bg);
                }
                else
                {
                    aso.CreateFrame().SetGlyph(0, 0, 0);
                    aso.Frames[fc].SetBackground(0, 0, fg);
                }
            }

            aso.AnimationStateChanged += (s, e) =>
            {
                if (e.NewState == AnimatedScreenObject.AnimationState.Finished)
                {
                    aso.CurrentFrameIndex = 0;
                }
            };

            return aso;
        }

        public static AnimatedEffect ExplosionEffect(int radius, TimeSpan speed)
        {
            // makes the size of the explosion always odd so that it's centered
            AnimatedEffect ef = new("explosion", (radius * 2) - 1, (radius * 2) - 1, TimeSpan.Zero, (0, 0));
            ef.FontSize = FontSize;
            ef.Repeat = false; // stop after playing once
            ef.AnimationDuration = speed;
            ef.Center = (radius, radius);

            Random rand = new();

            char blast = '#';
            char[] particles = { '*', '+', '.', '^', 'o', 'O', '0' };

            // set center
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
                foreach(var point in Algorithms.GetPointsInCircumference(++r, (radius, radius))) 
                {
                    ef.Frames[fc].SetGlyph(point.X, point.Y, blast);
                    ef.Frames[fc].SetForeground(point.X, point.Y, Color.Orange);
                }

                // then put random particles inside the circle.
                // the formula is ceil(2 ^ (x / 1.6) - 2)
                int numberOfPoints = (int)Math.Ceiling(Math.Pow(2, radius/1.6f) - 2); 
                var innerPoints = Algorithms.GetRandomPointsInCircle(r - 1, (radius, radius), numberOfPoints);
                foreach (var point in innerPoints)
                {
                    char particle = particles[rand.Next(particles.Length)];
                    ef.Frames[fc].SetGlyph(point.X, point.Y, particle);
                    ef.Frames[fc].SetForeground(point.X, point.Y, Color.Gray);
                }
            }

            // lastly, add an empty frame
            ef.CreateFrame();


            return ef;
        }

        public static AnimatedEffect TestEffect(int radius)
        {
            AnimatedEffect effect = new("asdf", radius * 2, radius * 2, TimeSpan.MaxValue, (radius, radius));
            effect.FontSize = FontSize;
            effect.Center = (radius, radius);
            effect.CreateFrame();
            effect.Repeat = true;
            effect.Frames[0].SetGlyph(0, 0, 'N');
            effect.Frames[0].DefaultBackground = Color.White;
            effect.AnimationDuration = TimeSpan.MaxValue;

            return effect;
        }

        public static AnimatedEffect Lightning(int radius, TimeSpan speed)
        {
            AnimatedEffect ef = new("lightning", (radius * 2) - 1, (radius * 2) - 1, TimeSpan.Zero, (0, 0));
            char lightning = (char)226; // capital gamma looks good enough

            ef.FontSize = FontSize;
            ef.Repeat = false; // stop after playing once
            ef.AnimationDuration = TimeSpan.FromSeconds(1);
            ef.Center = (radius, radius);

            Random rand = new();

            Func<List<Point>> func = () =>
            {
                List<Point> points = new();
                for (int x = 0; x < (radius * radius / 2); x++)
                {
                    int a = rand.Next((radius * 2) - 1);
                    int y = rand.Next((radius * 2) - 1);
                    points.Add((a, y));
                }

                return points;
            };

            for (int i = 0; i < 10; i++)
            {
                var points = func();
                ef.CreateFrame();
                foreach(Point p in points)
                {
                    ef.Frames[i].SetGlyph(p.X, p.Y, lightning);
                }

                ef.Frames[i].Surface.DefaultForeground = Color.LightBlue;
                //ef.Frames[i].Surface.DefaultForeground = Color.Red;
            }

            //Logger.Report("lightnign effect", $"Contains {ef.Frames.Count} frames");
            ef.CreateFrame();
            return ef;
        }
    }
}
