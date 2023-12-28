using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Objects
{
    public class MagicCircle : StaticGameObject
    {
        private GameObject owner;

        private static AnimatedScreenObject prepareAppearance(Color color)
        {
            AnimatedScreenObject aso = new("circle", 1, 1);
            aso.CreateFrame()[0].Foreground = color;
            aso.Frames[0].SetGlyph(0, 0, '@');

            return aso;
        }

        public MagicCircle(Color color, Alignment alignment, GameObject owner) : base(prepareAppearance(color), -1)
        {
            Type = Type.CIRCLE;
            Alignment = alignment;
            this.owner = owner;
        }
    }
}
