using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Objects.Classes
{
    // not problems like what is beauty
    public class Engineer : ControllableGameObject
    {
        public Engineer(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
        }
    }
}
