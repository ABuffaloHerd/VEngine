using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Effects;

namespace VEngine.Objects
{
    internal class EffectManager
    {
        private List<Effect> effects;
        public EffectManager() 
        { 
            effects = new();
        }
    }
}
