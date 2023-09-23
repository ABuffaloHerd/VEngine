using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Void;
using SadConsole;
using SadConsole.Entities;
using Void.Item.Weapon;
using Void.DataStructures;

namespace Void.Battle
{
    public class Effect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Timer { get; private set; }

        // This function takes in the target object and returns a descriptive string of what it just did to said object
        Func<GameObject, string> OnApply;

        public string OnTrigger(GameObject target, string wtfhappened)
        {
            return OnApply(target);
        }
    }
}