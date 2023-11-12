using SadConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Scenes
{
    public partial class CombatScene : Scene
    {
        private void SetupArena()
        {
            arena.Surface.DefaultBackground = Color.Red;
            arena.Surface.DefaultForeground = Color.Black;
            arena.Surface.FillWithRandomGarbage(Game.Instance.DefaultFont);
            Border.BorderParameters b = Border.BorderParameters.GetDefault().AddTitle("ARENA");
            new Border(arena, b);
        }
    }
}
