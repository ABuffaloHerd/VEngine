using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.AI;
using VEngine.Animations;
using VEngine.Data;
using VEngine.Objects;
using VEngine.Objects.Classes;

namespace VEngine.Scenes
{
    internal static class ScenarioPresets
    {
        public static CombatScenario CombatTest;
        static ScenarioPresets()
        {
            CombatTest = new("combat_test", "testing classes", 24, 24);

            // define some objects

            /* ===== Test code ===== */
            AnimatedScreenObject animated = new("Targetdummy", 1, 1);
            animated.CreateFrame()[0].Glyph = 'T';

            AIControlledGameObject test = new(animated, 1, new ZombieAI());
            test.Name = "targetdummy";
            test.Position = (7, 4);
            test.HP = int.MaxValue;

            AnimatedScreenObject aso = new("Controllable", 1, 1);
            aso.CreateFrame()[0].Glyph = (char)1;
            ControllableGameObject pgo = new(aso, 2);
            pgo.Name = "Controllable";
            pgo.Speed = 250;

            // put some walls
            AnimatedScreenObject wallobj = new("wall", 1, 1);
            wallobj.CreateFrame()[0].Glyph = (char)219;

            StaticGameObject wall = new(wallobj, 1);
            wall.Position = (3, 3);

            StaticGameObject wall2 = new(wallobj, 1)
            {
                Position = (3, 2)
            };

            StaticGameObject wall3 = new(wallobj, 1)
            {
                Position = (3, 4)
            };

            AnimatedScreenObject aso2 = AnimationPresets.BlinkingEffect("Ranger", 'M', Color.Gray, Color.Black, (1, 1));
            Ranger ranger = new(aso2, 1);
            ranger.Name = "Minako";
            ranger.Speed = 120;
            ranger.Position = (0, 4);

            AnimatedScreenObject aso3 = AnimationPresets.BlinkingEffect("Mage", 'S', Color.PaleGoldenrod, Color.Black, (1, 1));
            Mage mage = new(aso3, 1);
            mage.Name = "Saki";
            mage.Speed = 90;
            mage.Position = (0, 3);

            AnimatedScreenObject aso4 = AnimationPresets.BlinkingEffect("Mage2", 'E', Color.LightGray, Color.Black, (1, 1));
            Mage mage2 = new(aso4, 1);
            mage2.Name = "Elaine";
            mage2.Speed = 101;
            mage2.Position = (0, 5);
            mage2.RES = 50;
            mage2.DEF = 2;
            mage2.MP = 300;

            AnimatedScreenObject aso5 = AnimationPresets.BlinkingEffect("Guard", 'H', Color.Red, Color.Black, (1, 1));
            Guard guard = new(aso5, 1);
            guard.Name = "Hirina";
            guard.Speed = 100;
            guard.Position = (0, 2);

            AnimatedScreenObject aso6 = AnimationPresets.BlinkingEffect("Vanguard", 'M', Color.SteelBlue, Color.Black, (1, 1));
            Vanguard vanguard = new(aso6, 1);
            vanguard.Name = "Mariah";
            vanguard.Speed = 190;
            vanguard.Position = (0, 6);

            CombatTest.Objects = new()
            {
                test,
                ranger,
                //wall,
                //wall2,
                //wall3,
                mage,
                mage2,
                guard,
                vanguard
            };

            CombatScenario.SerializeJSON(CombatTest);
        }
    }
}
