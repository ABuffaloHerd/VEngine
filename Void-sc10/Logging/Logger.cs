using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Logging
{
    public static class Logger
    {
        public static void Report(object sender, string text)
        {
            StringBuilder sb = new();
            sb.Append(sender.ToString())
                .Append(" : ")
                .Append(text);

            System.Console.WriteLine(sb.ToString());
        }
    }
}
