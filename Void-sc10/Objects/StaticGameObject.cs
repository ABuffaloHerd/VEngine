using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Scenes.Combat;

namespace VEngine.Objects
{
    /// <summary>
    /// Walls need collision too.
    /// </summary>
    public class StaticGameObject : GameObject
    {

        public StaticGameObject(AnimatedScreenObject appearance, int zlevel) : base(appearance, zlevel)
        {
            MoveDist = 0;
            HP = int.MaxValue;
            Speed = 0;

            IsStatic = true;
        }

        /// <summary>
        /// doesn't do shit
        /// </summary>
        /// <param name="dest"></param>
        public sealed override void Attack(IEnumerable<GameObject> targets, Arena arena)
        {
            // don't do shit
        }

        /// <summary>
        /// <inheritdoc cref="Attack(IEnumerable{GameObject}, Arena)"/>
        /// </summary>
        /// <param name="dest"></param>
        public sealed override void Move(Point dest)
        {
            // don't do shit
        }

        /// <summary>
        /// <inheritdoc cref="Attack(IEnumerable{GameObject}, Arena)"/>
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="type"></param>
        public sealed override int TakeDamage(int damage, DamageType type)
        {
            return 0;
        }

        /// <summary>
        /// <inheritdoc cref="Attack(IEnumerable{GameObject}, Arena)"/>
        /// </summary>
        /// <returns></returns>
        public sealed override ICollection<ControlBase> GetHudElements()
        {
            return new List<ControlBase>(); // return an empty list
        }
    }
}
