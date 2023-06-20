using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadTutorial.Colour
{
    public static class ColourManipulation
    {
        public static ColoredString MakeStringDarker(this ColoredString instance)
        {

            for (int i = 0; i < instance.Length; i++)
            {
                instance[i].Foreground = instance[i].Foreground.GetDarker();
            }

            return instance;
        }
    }
}
