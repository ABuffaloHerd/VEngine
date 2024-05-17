using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Objects;
using VEngine.Scenes.Combat;
using VEngine.Data;
using Algorithms = VEngine.Data.Algorithms;

namespace VEngine.AI
{
    public class ZombieAI : IAIActor
    {
        private GameObject parent;
        public IEnumerable<AIAction> GetAIActions()
        {
            throw new NotImplementedException();
        }

        public AIAction GetNextAction()
        {
            throw new NotImplementedException();
        }

        public bool HasNextAction()
        {
            throw new NotImplementedException();
        }

        public void SetParent(GameObject parent)
        {
            this.parent = parent;
        }

        public void UpdateAI(Arena arena)
        {
            // get closest enemy
            GameObject closest = Algorithms.Closest(arena.GetEntities(Alignment.FRIEND), parent.Position);
            if (closest == null) 
            {
                // shit yourself because i don't know what to do anymore.
            }


        }
    }
}
