using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Objects;

namespace VEngine.Scenes
{
    /// <summary>
    /// This class contains information on how to set up a combat scene.
    /// A combat scene needs to know which objects are involved. These are IControllable objects.
    /// Other objects (except walls) have yet to be implemented.
    /// 
    /// The scenario also defines what special effects this combat scene has, such as posionous fog, or meteor strikes.
    /// </summary>
    public class CombatScenario
    {
        public string Name {  get; set; }
        public string Description { get; set; }
        public List<GameObject> Objects;

        public int ArenaWidth { get; set; }
        public int ArenaHeight { get; set; }

        public CombatScenario(string name, string description, int arenaWidth, int arenaHeight)
        {
            Objects = new();
            ArenaWidth = arenaWidth;
            ArenaHeight = arenaHeight;
        }
    }
}
