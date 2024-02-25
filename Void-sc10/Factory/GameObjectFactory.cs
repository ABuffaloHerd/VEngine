using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.AI;
using VEngine.Animations;
using VEngine.Objects;
using VEngine.Objects.Classes;

namespace VEngine.Factory
{
    /// <summary>
    /// Factory class that creates... controllable game objects.
    /// </summary>
    public static class ControllableGameObjectFactory
    {
        public static ControllableGameObject CreateControllableGameObject(string name, char glyph, Color colour, Point position,
            int speed = 100, int hp = 10, int mp = 10, int sp = 10, int def = 0, int res = 0
            )
        {
            AnimatedScreenObject appearance = AnimationPresets.BlinkingEffect(name, glyph, colour, Color.TransparentBlack, (1, 1));
            ControllableGameObject obj = new(appearance, 1)
            {
                Name = name,
                Speed = speed,
                HP = hp,
                MP = mp,
                SP = sp,
                DEF = def,
                RES = res,
                Position = position
            };

            return obj;
        }
    }

    /// <summary>
    /// Can YOU think of a more cumbersome name?
    /// </summary>
    public static class AIControlledGameObjectFactory
    {
        public static AIControlledGameObject CreateAIControlledGameObject(string name, char glyph, Color fg, Point position, IAIActor behavior,
            int speed = 100, int hp = 10, int def = 0, int res = 0
            )
        {
            AnimatedScreenObject aso = new(name, 1, 1);
            aso.CreateFrame()[0].Glyph = glyph;
            aso.Frames[0].SetForeground(0, 0, fg);
            AIControlledGameObject thing = new(aso, 1, behavior)
            {
                Name = name,
                Speed = speed,
                HP = hp,
                DEF = def,
                RES = res,
                Position = position
            };

            return thing;
        }
    }


    /// <summary>
    /// YEARS of objects yet NO ADVANTAGES FOUND OVER STRUCTS <br></br>
    /// we had a tool for putting various types of data in one place. it was called <c>void*</c>
    /// </summary>
    public static class WallFactory
    {
        public static StaticGameObject CreateWall(Point position)
        {
            AnimatedScreenObject aso = new AnimatedScreenObject("wall", 1, 1);
            aso.CreateFrame()[0].Glyph = (char)219;
            StaticGameObject wall = new StaticGameObject(aso, 1)
            {
                Position = position
            };

            return wall;
        }
    }
}
