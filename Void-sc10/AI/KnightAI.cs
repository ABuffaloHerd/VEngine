using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.AI.Pathfinder;
using VEngine.Logging;
using VEngine.Objects;
using VEngine.Scenes.Combat;

using Algorithms = VEngine.Data.Algorithms;

namespace VEngine.AI
{
    public class KnightAI : ZombieAI
    {
        private GameObject parent;
        private Queue<AIAction> actions = new();

        new public void UpdateAI(Arena arena)
        {
            Logger.Report(this, "Knight AI update");

            // get closest enemy
            GameObject? closest = Algorithms.Closest(arena.GetEntities(Alignment.FRIEND), parent.Position);
            if (closest == null)
            {
                // shit yourself because i don't know what to do anymore.
                Logger.Report(this, "FindClosest failed");
                return;
            }

            // check the closest point found by the algorithm
            Point dest = ClosestPointFinder.FindClosestPoint(parent.Position, parent.Speed, closest.Position);

            // pathfind to this location using the arena's pathfinder.

        }
    }
}
