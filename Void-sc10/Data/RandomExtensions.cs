using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Data
{
    public static class RandomExtensions
    {
        public static T RandomEnum<T>(this Random rand)
            where T : struct, Enum
        {
            var values = Enum.GetValues<T>();
            return values[rand.Next(values.Length)];
        }
    }
}
