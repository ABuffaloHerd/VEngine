using SadConsole.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.AI;
using VEngine.Data;
using VEngine.Logging;
using VEngine.Objects;

namespace VEngine.Scenes.Combat
{
    public partial class CombatScene
    {
        public void HandleAI(AIControlledGameObject thing)
        {
            Logger.Report(this, "Handling AI");
            // Update AI state
            thing.UpdateAI(arena);

            // get thing's action
            while(thing.AI.HasNextAction())
            {
                Logger.Report(this, "AI has next action");
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

                    case AIActionType.ATTACK:
                        ExecuteAttack(thing, thing.Range);
                        break;

                    case AIActionType.TELEPORT:
                        // guard against putting the same object in the same place.
                        if (!arena.IsTileFree((action.data as TeleportActionData).Destination)) break;
                        selectedGameObject.Position = (action.data as TeleportActionData).Destination;
                        break;

                }

                Logger.Report(this, "Action complete, waiting");
            }

            OnNextTurn();
        }
    }
}
