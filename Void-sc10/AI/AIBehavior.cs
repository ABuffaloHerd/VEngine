using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Logging;
using VEngine.Scenes.Combat;

namespace VEngine.AI
{
    public class TestAIBehavior : IAIActor
    {
        private readonly List<AIAction> aiActions = new();
        private int test = 0;

        public IEnumerable<AIAction> GetAIActions()
        {
            return aiActions;
        }

        public AIAction GetNextAction()
        {
            if (aiActions.Any()) // not empty
            {
                AIAction action = aiActions.First();
                aiActions.RemoveAt(0);
                return action;
            }

            return new(AIActionType.MOVE, new MoveActionData(new Random().RandomEnum<Data.Direction>()));
        }

        public void UpdateAI(Arena arena)
        {
            Logger.Report(this, "AI state update!");

            IAIActionData data = new MoveActionData(new Random().RandomEnum<Data.Direction>());
        }

        public bool HasNextAction()
        {
            if (test < 10)
            {
                test++;
                return true;
            }
            else
            {
                test = 0;
                return false; 
            }
        }
    }
}
