using SadConsole.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Logging;
using VEngine.Objects;

namespace VEngine.Scenes.Combat
{
    /// <summary>
    /// Contains methods related to interactions between game objects.
    /// </summary>
    public partial class CombatScene
    {
        /// <summary>
        /// Sends a request to attack and a list of objects to the attacker
        /// </summary>
        /// <param name="attacker">Attacker</param>
        /// <param name="range">Which tiles to check (automatically applies offset)</param>
        public void ExecuteAttack(GameObject attacker, Pattern range)
        {
            // From the given range, give possible targets in a collection to the attacker
            List<GameObject> targets = arena.GetInPattern(range, selectedGameObject.Position);
            targets.Remove(selectedGameObject);

            foreach (GameObject target in targets) 
            {
                Logger.Report(this, target.ToString());
            }
            attacker.Attack(targets);

            Logger.Report(this, "attack executed");
        }
    }
}
