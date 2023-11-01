using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Void.DataStructures;

namespace Void.Scene
{
    public class DebugScene : Scene
    {
        // put code in the constructor to use as console output for testing
        public DebugScene()
        {
            DataStructures.Pattern r = new();

            r.Mark(0, 0);
            r.Mark(1, 0);

            System.Console.WriteLine(r.ToString());
        }
        public override void Render()
        {
            
        }
    }
}
