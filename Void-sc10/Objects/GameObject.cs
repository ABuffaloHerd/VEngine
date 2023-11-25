using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VEngine.Data;

namespace VEngine.Objects
{
    public class GameObject : Entity
    {
        public Stat Speed { get; set; }
        public Direction Looking { get; set; }

        private List<Effect> effects;


        /// <summary>
        /// Creates a game object to be used in combat scenarios
        /// </summary>
        /// <param name="appearance">
        /// AnimatedScreenObject with frame data used for the blinking effect. Should be part 
        /// of the character class.
        /// </param>
        /// <param name="zIndex"></param>
        public GameObject(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            effects = new();
            Speed = (Stat)100;
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
