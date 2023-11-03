using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Events
{
    public interface IGameEvent
    {
        IGameEvent AddData(string key, object value);
        bool Contains(string key);
        T GetData<T>(string key);
    }
}
