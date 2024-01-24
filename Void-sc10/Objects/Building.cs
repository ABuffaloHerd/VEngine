using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Objects
{
    public class Building : ControllableGameObject
    {
        /// <summary>
        /// How many times it must be hit to be constructed
        /// </summary>
        public int Steps { get; set; } = 3;
        public bool IsActive 
        {
            get => stage == Steps; 
        }
        private int stage;
        public Building(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            stage = 0;
        }

        public void Build()
        {
            stage++;
        }
    }
}
