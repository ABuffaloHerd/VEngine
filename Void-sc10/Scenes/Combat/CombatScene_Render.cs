using SadConsole.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Logging;

namespace VEngine.Scenes.Combat
{
    /// <summary>
    /// Contains methods relating to rendering on this scene
    /// </summary>
    public partial class CombatScene
    {
        // TODO: Move this to the arena class.
        /// <summary>
        /// Tells the arena to put blinking Xs where the current game object is.
        /// </summary>
        /// <param name="p">pattern to render</param>
        /// <param name="offset">offset of the pattern</param>
        private void RenderPattern(Pattern p, Point offset)
        {
            Blinker b = new()
            {
                // Blink forever
                Duration = System.TimeSpan.MaxValue,
                BlinkOutForegroundColor = Color.Black,
                // Every half a second
                BlinkSpeed = TimeSpan.FromMilliseconds(500),
                RunEffectOnApply = true
            };

            foreach(var point in p)
            {
                Logger.Report(this, "point found");
                Point newOffset = point + offset;
                arena.Surface.SetForeground(newOffset.X, newOffset.Y, Color.Yellow);
                arena.Surface.SetGlyph(newOffset.X, newOffset.Y, 'X');
                arena.Surface.SetEffect(newOffset.X, newOffset.Y, b);
            }
        }
    }
}
