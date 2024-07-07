using SadRogue.Primitives;
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

            // check if closest is right next to me
            var directions = new (int X, int Y)[]
            {
                (-1, 0),    // Left
                (1, 0),     // Right
                (0, 1),     // Up
                (0, -1)     // Down
            };

            foreach (var direction in directions)
            {
                Point pos = (parent.Position - direction);
                GameObject? target = arena.At(pos.X, pos.Y);

                if (target != null && target.Alignment == Alignment.FRIEND && target is not StaticGameObject)
                {
                    // calculate the direction to face
                    Point directionVector = new(
                        parent.Position.X - closest.Position.X,
                        parent.Position.Y - closest.Position.Y
                    );

                    // Use the FromVector extension to determine the facing direction
                    Data.Direction facing = directionVector.ToDirection();
                    AttackActionData data = new(facing);
                    AIAction attack = new(AIActionType.ATTACK, data);

                    Logger.Report(this, $"attacking {facing}, direction vector is {directionVector}");
                    actions.Enqueue(attack);
                    return; // no need to move
                }
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
