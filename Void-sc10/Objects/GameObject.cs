using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Objects
{
    public class GameObject : Entity
    {



        private List<Effect> effects;


        /// <summary>
        /// Creates a game object to be used in combat scenarios
        /// </summary>
        /// <param name="appearance">
        /// AnimatedScreenObject with frame data used for the blinking effect. Should be part 
        /// of the character class.
        /// </param>
        /// <param name="zIndex"></param>
        public GameObject(Animated appearance, int zIndex) : base(appearance, zIndex)
        {
            effects = new();
        }

        public virtual void Update()
        {
            foreach(Effect ef in effects)
            {
                ef.Apply(this);
            }
        }
    }
}
