using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VEngine.Objects;
using VEngine.Scenes.Combat;

namespace VEngine.AI
{
    /// <summary>
    /// Arena state -> UpdateAI -> GetNextAction -> Scene executes
    /// Scene is responsible for executing AI's actions
    /// </summary>
    public interface IAIActor
    {
        /// <summary>
        /// Updates internal ai state based on arena state. Responsible for creating new actions that the object will take during the execution step.
        /// </summary>
        /// <param name="arena"></param>
        void UpdateAI(Arena arena);

        /// <summary>
        /// Allows the implementing class to access its parent
        /// </summary>
        /// <param name="parent"></param>
        void SetParent(GameObject parent);

        /// <summary>
        /// Gets the next chosen action
        /// </summary>
        /// <returns>Action</returns>
        AIAction GetNextAction();

        /// <summary>
        /// Are there more?
        /// </summary>
        /// <returns>use your head</returns>
        bool HasNextAction();

        /// <summary>
        /// Gets a sequence of incoming actions
        /// </summary>
        /// <returns></returns>
        IEnumerable<AIAction> GetAIActions();
    }

    public enum AIActionType
    {
        NONE = 0,
        RELINQUISH,
        ATTACK,
        CAST,
        BUFF,
        DEFEND,
        PARRY,
        MOVE,
        TELEPORT
    }

    public static class AIActorExtensions
    {
        public static Point CheckAndBounce(this IAIActor actor, Arena arena, Point dest)
        {
            var directions = new (int X, int Y)[]
            {
                (-1, 0),    // Left
                (1, 0),     // Right
                (0, 1),     // Up
                (0, -1)     // Down
            };

            if (!arena.IsTileFree(dest))
            {
                foreach (var direction in directions)
                {
                    var newDest = (dest.X + direction.X, dest.Y + direction.Y);
                    if (arena.IsTileFree(newDest))
                    {
                        return newDest;
                    }
                }
            }

            return dest; // return original if it failed
        }

        public static List<Point> LimitMovement(List<Point> path, int maxSteps)
        {
            if (path.Count <= maxSteps)
                return path;

            return path.GetRange(0, maxSteps);
        }
    }
}
