using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            System.Console.WriteLine("Area constructor works");
        }

        public void Render()
        {
            // you best set up your positions proper before this is called.
            // function must copy object appearance to target surface.

            foreach(GameObject obj in objects)
            {
                
            }
        }

        private void CopyAppearance(GameObject obj) 
        {
            Surface.SetGlyph(obj.Position.X, obj.Position.Y, obj.AppearanceSingle);
        }
    }
}
