using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Battle
{
    public abstract partial class GameObject
    {
        private List<Effect> Effects;

        public virtual void ApplyEffects()
        {
            throw new NotImplementedException();
        }
    }
}
