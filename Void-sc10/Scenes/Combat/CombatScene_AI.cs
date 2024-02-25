using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.AI;
using VEngine.Data;
using VEngine.Logging;
using VEngine.Objects.Classes;

namespace VEngine.Scenes.Combat
{
    public partial class CombatScene
    {
        public void HandleAI(AIControlledGameObject thing)
        {
            Logger.Report(this, "Handling AI");
            // get thing's action
            AIAction action = thing.GetNextAction();

            // figure out what action it is
            switch(action.ActionType)
            {
                // these two pass the turn to the next object without doing anything.
                case AIActionType.NONE:
                case AIActionType.RELINQUISH:
                    OnNextTurn();
                    break;

                case AIActionType.MOVE:
                    // get data
                    Data.Direction direction = (action.data as MoveActionData).Direction;
                    Move(direction.ToVector());
                    break;
            }

            OnNextTurn();
        }
    }
}
