using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Objects;

namespace VEngine.Scenes.Combat
{
    /// <summary>
    /// This class handles everything to do with rendering entities and checking collision
    /// </summary>
    internal class Arena : Console
    {
        public EntityManager EntityManager { get; private set; }
        public Arena(int width, int height) : base(width, height)
        {
            EntityManager EntityManager = new();

            SadComponents.Add(EntityManager);

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    Surface.SetForeground(x, y, Color.Gray);
                    Surface.SetGlyph(x, y, '+');
                }
            }
        }

        public void AddEntity(GameObject gameObject)
        {
            EntityManager.Add(gameObject);
        }
    }
}
