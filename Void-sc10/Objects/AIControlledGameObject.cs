using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.AI;
using VEngine.Components;
using VEngine.Scenes.Combat;

namespace VEngine.Objects
{
    /// <summary>
    /// base class for all AI controlled game objects.
    /// </summary>
    public class AIControlledGameObject : GameObject
    {
        public IAIActor AI { get; protected set; }
        public AIControlledGameObject(AnimatedScreenObject appearance, int zIndex, IAIActor ai)
            : base(appearance, zIndex)
        {
            AI = ai;
            ai.SetParent(this);

            Alignment = Alignment.ENEMY; // default for all AI controleld things

            AddComponent(new CollisionComponent());
        }

        /// <summary>
        /// Updates the AI state given an arena
        /// </summary>
        /// <param name="a">The arena to get info from</param>
        public virtual void UpdateAI(Arena a)
        {
            AI.UpdateAI(a);
        }

        public virtual AIAction GetNextAction()
        {
            return AI.GetNextAction();
        }
    }
}
