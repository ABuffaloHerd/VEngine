using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.DataStructures;
using Void.UI;

namespace Void.Battle
{
    public class Arena : ScreenSurface, IRenderable
    {
        public List<GameObject> objects;
        public Arena(int width, int height, Color background) : base(width, height)
        {
            objects = new();

            _ = Surface.Fill(background: background);
        }

        public void Mark(Pattern p)
        {
            foreach(Point point in p.Points)
            {
                Surface.SetGlyph(point.X, point.Y, 88, Color.Yellow);
            }
        }

        public void Mark(Pattern p, Point position)
        {
            foreach(Point point in p.Points)
            {
                int x = point.X + position.X;
                int y = point.Y + position.Y;

                Surface.SetGlyph(x, y, 88, Color.Yellow);
            }
        }

        public void Render()
        {
            // you best set up your positions proper before this is called.
            // function must copy object appearance to target surface.

            foreach(GameObject obj in objects)
            {
                
            }
        }
    }
}
