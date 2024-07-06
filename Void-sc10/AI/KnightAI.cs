using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.AI.Pathfinder;
using VEngine.Data;
using VEngine.Logging;
using VEngine.Objects;
using VEngine.Scenes.Combat;

using Algorithms = VEngine.Data.Algorithms;

namespace VEngine.AI
{
    public class KnightAI : IAIActor
    {
        private GameObject parent;
        private Queue<AIAction> actions = new();
        public IEnumerable<AIAction> GetAIActions()
        {
            return actions.ToList();
        }

        public AIAction GetNextAction()
        {
            return actions.Dequeue();
        }

        public bool HasNextAction()
        {
            return actions.Count > 0;
        }

        public void SetParent(GameObject parent)
        {
            this.parent = parent;
        }

        public void UpdateAI(Arena arena)
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
            Point dest = ClosestPointFinder.FindClosestPoint(parent.Position, parent.MoveDist, closest.Position);

            // pathfind to this location using the pathfinder.
            Graph arenaGraph = arena.ToGraph();
            List<Point> path = DijkstraPathFinder.FindPath(arenaGraph, parent.Position, dest);

            var ltdpath = AIActorExtensions.LimitMovement(path, parent.MoveDist);

            foreach(var p in ltdpath) 
            {
                AIAction act = new(AIActionType.TELEPORT, new TeleportActionData(p));
                actions.Enqueue(act);
            }
        }
    }
}
