using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.AI
{
    public interface IAIActionData { }

    public class NoActionData : IAIActionData { };

    public class MoveActionData : IAIActionData 
    {
        public Data.Direction Direction { get; private set; }
        public MoveActionData(Data.Direction direction) 
        {
            Direction = direction;
        }
    }
}
