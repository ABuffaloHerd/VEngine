using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Objects;
using VEngine.Scenes.Combat;

namespace VEngine.Items
{
    public interface ICombatItem
    {
        string Name { get; set; }
        string Description { get; set; }
        CombatEvent ApplyEffect(IEnumerable<GameObject> targets, GameObject wielder, Arena arena);
    }
}
