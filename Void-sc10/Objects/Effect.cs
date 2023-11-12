using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Objects
{
    public class Effect
    {
        public int Timer { get; private set; }
        public string Name { get; private set; }

        public void Apply(GameObject target)
        {
            Timer--;
        }
    }
}
