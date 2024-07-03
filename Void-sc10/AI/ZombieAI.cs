using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Objects;
using VEngine.Scenes.Combat;
using VEngine.AI.Pathfinder;
using VEngine.Data;
using Algorithms = VEngine.Data.Algorithms;
using VEngine.Logging;

namespace VEngine.AI
{
    public class ZombieAI : IAIActor
    {
        protected GameObject parent;
        protected Queue<AIAction> actions = new();
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
            Logger.Report(this, "zombie ai update");

            var directions = new (int X, int Y)[]
            {
                (-1, 0),  // Left
                (1, 0),   // Right
                (0, -1),  // Up
                (0, 1)    // Down
            };

            // get closest enemy
            GameObject? closest = Algorithms.Closest(arena.GetEntities(Alignment.FRIEND), parent.Position);
            if (closest == null) 
            {
                // shit yourself because i don't know what to do anymore.
                Logger.Report(this, "FindClosest failed");
                return;
            }

            // check if closest is right next to me
            foreach(var direction in directions)
            {
                Point pos = (parent.Position + direction);
                GameObject? target = arena.At(pos.X, pos.Y);

                if (target != null && target.Alignment == Alignment.FRIEND)
                {
                    // what a mess
                    AttackActionData data = new(new Point(direction.X, direction.Y).FromVector());
                    AIAction attack = new(AIActionType.ATTACK, data);

                    actions.Enqueue(attack);
                    return; // you're done. good job.
                }
            }

            // check the closest point found by the algorithm
            Point dest = ClosestPointFinder.FindClosestPoint(parent.Position, parent.Speed, closest.Position);


            if (!arena.IsTileFree(dest))
            {
                foreach (var direction in directions)
                {
                    var newDest = (dest.X + direction.X, dest.Y + direction.Y);
                    if (arena.IsTileFree(newDest))
                    {
                        dest = newDest;
                        break;
                    }
                }
            }

            //parent.Position = dest;
            AIAction action = new(AIActionType.TELEPORT, new TeleportActionData(dest));

            actions.Enqueue(action);
        }
    }
}
