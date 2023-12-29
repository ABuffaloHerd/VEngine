using SadConsole.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Items;
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
            List<GameObject> targets = arena.GetInPattern(range, selectedGameObject.Position, selectedGameObject.Facing);

            // remove the current object from hurting itself
            targets.Remove(selectedGameObject);

            // Do the attack. This sends an event to the scene with attack information.
            attacker.Attack(targets, arena);

            Logger.Report(this, "attack executed");
        }

        // Slightly different implementation for spells
        public void CastSpell(GameObject attacker, Pattern range, Spell s)
        {
            // From the given range, give possible targets in a collection to the attacker
            List<GameObject> targets = arena.GetInPattern(range, selectedGameObject.Position, selectedGameObject.Facing);

            // remove the current object from hurting itself
            targets.Remove(selectedGameObject);

            attacker.Cast(targets, arena, s);

            Logger.Report(this, "Spell cast!");
        }

        /// <summary>
        /// Performs bounds and stat checking before moving the selected game object
        /// </summary>
        /// <param name="direction">The direction to move in</param>
        private bool Move(Point direction)
        {
            bool yes = arena.IsTileFree(selectedGameObject.Position + direction);

            if (yes)
            {
                selectedGameObject.Move(direction);
            }

            return yes;
        }
    }
}
