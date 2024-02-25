using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.AI
{
    public record AIAction(AIActionType ActionType, IAIActionData data);
}
