using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Objects
{
    public enum Type
    {
        /// <summary>
        /// Meat puppets or walking robots
        /// </summary>
        ENTITY,

        /// <summary>
        /// Things you can't walk into
        /// </summary>
        WALL,

        /// <summary>
        /// Turrets, dispenser, mortars etc
        /// </summary>
        CONSTRUCT,

        /// <summary>
        /// [stands]
        /// </summary>
        SUMMON,

        /// <summary>
        /// magic circle
        /// </summary>
        CIRCLE
    }
}
