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
        private List<GameObject> objects;
        public Arena(int width, int height, Color background) : base(width, height)
        {
            actors = new();

            _ = Surface.Fill(background = background);
        }

        public void Render()
        {
            // you best set up your positions proper before this is called.
        }
    }
}
