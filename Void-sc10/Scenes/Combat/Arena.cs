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
            EntityManager = new();

            SadComponents.Add(EntityManager);
        }

        public void AddEntity(GameObject gameObject)
        {
            EntityManager.Add(gameObject);
        }

        public void ClearEffects()
        {
            foreach(ICellSurface cell in Surface)
            {
                Surface.SetEffect(cell, null);
            }
        }
    }
}
