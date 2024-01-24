using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;

namespace VEngine.Objects.Classes
{
    // not problems like what is beauty
    public class Engineer : ControllableGameObject
    {
        public Stat Metal { get; set; }
        public Engineer(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            // default metal is 50
            Metal = 50;
        }
    }
}
