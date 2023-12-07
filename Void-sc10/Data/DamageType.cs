using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Data
{
    public enum DamageType
    {
        /// <summary>
        /// testificate
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Flat reduction via DEF stat
        /// </summary>
        PHYSICAL, 

        /// <summary>
        /// Percentage reduction via RES stat
        /// </summary>
        MAGIC,

        /// <summary>
        /// ouch
        /// </summary>
        TRUE
    }
}
