using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Objects;

namespace VEngine.Components
{
    public abstract class Component
    {
        public GameObject parent;
    }

    public class CollisionComponent : Component
    {
        public int Weight = 1;
    }
}
