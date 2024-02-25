using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Scenes.Combat;

namespace VEngine.AI
{
    /// <summary>
    /// Arena state -> UpdateAI -> GetNextAction()
    /// Scene is responsible for executing AI's actions
    /// </summary>
    public interface IAIActor
    {
        /// <summary>
        /// Updates internal ai state based on arena state.
        /// </summary>
        /// <param name="arena"></param>
        void UpdateAI(Arena arena);

        /// <summary>
        /// Gets the next chosen action
        /// </summary>
        /// <returns>Action</returns>
        AIAction GetNextAction();

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
        MOVE
    }
}
