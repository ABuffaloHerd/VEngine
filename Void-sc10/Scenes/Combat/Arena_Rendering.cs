using SadConsole.Effects;
using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Logging;
using VEngine.Objects;

namespace VEngine.Scenes.Combat
{
    /// <summary>
    /// Thanks Thraka
    /// </summary>
    public partial class Arena
    {
        private SadConsole.Renderers.IRenderStep _secondSurfaceRenderStep;
        private ScreenSurface _secondSurfaceWrapper;

        public ICellSurface SecondSurface => _secondSurfaceWrapper.Surface; // This surface is used for the overlay.
        protected void SetupRenderer()
        {
            // Create the top layer that's above entities
            _secondSurfaceWrapper = new(this.Surface.Width, this.Surface.Height);
            //_secondSurfaceWrapper.Parent = this; // parenting
            _secondSurfaceWrapper.Surface.DefaultBackground = Color.Transparent;
            _secondSurfaceWrapper.FontSize = this.FontSize;

            // Create the new render step and tell it to render the second surface
            _secondSurfaceRenderStep = new SadConsole.Renderers.SurfaceTargetRenderStep();
            _secondSurfaceRenderStep.SetData(_secondSurfaceWrapper);
            //_secondSurfaceWrapper.Position = (0, 0);

            // Add the new render step to the current renderer for this ScreenSurface
            Renderer!.Steps.Add(_secondSurfaceRenderStep);
        }

        protected override void OnFontChanged(IFont oldFont, Point oldFontSize)
        {
            // If the font changes in this surface object, update the second surface.
            _secondSurfaceWrapper.Font = Font;
            _secondSurfaceWrapper.FontSize = FontSize;
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
            _secondSurfaceWrapper.Update(delta);
            _secondSurfaceWrapper.ViewPosition = this.ViewPosition;
        }

        protected override void Dispose(bool disposing)
        {
            _secondSurfaceRenderStep = null!;
            _secondSurfaceWrapper.Dispose();
            _secondSurfaceWrapper = null!;
            base.Dispose(disposing);
        }

        /// <summary>
        /// Tells the arena to put blinking Xs where the current game object is.
        /// </summary>
        /// <param name="p">pattern to render</param>
        /// <param name="offset">offset of the pattern</param>
        public void RenderPattern(Pattern p, Point offset, Data.Direction direction)
        {
            StopRenderPattern();

            Blinker b = new()
            {
                // Blink forever
                Duration = System.TimeSpan.MaxValue,
                BlinkOutForegroundColor = Color.TransparentBlack,
                // Every half a second
                BlinkSpeed = TimeSpan.FromMilliseconds(500),
                RunEffectOnApply = true
            };

            // turn enum direction into a number that the pattern recognises as a valid rotation
            foreach (var point in p.GetRotated((int)direction))
            {
                Point newOffset = point + offset;
                Logger.Report(this, $"setting glyph at position {newOffset}");
                SecondSurface.SetForeground(newOffset.X, newOffset.Y, Color.Yellow);
                SecondSurface.SetGlyph(newOffset.X, newOffset.Y, 'X');
                SecondSurface.SetEffect(newOffset.X, newOffset.Y, b);
            }

            IsRenderingPattern = true;
            currentPattern = p; //save current pattern
        }

        /// <summary>
        /// Returns a list of objects found with the given pattern
        /// </summary>
        /// <param name="p">pattern</param>
        /// <param name="offset">offset to shift tile checking by</param>
        /// <returns></returns>
        public List<GameObject> GetInPattern(Pattern p, Point offset, Data.Direction direction)
        {
            List<GameObject> list = new();

            foreach (Point point in p.GetRotated((int)direction))
            {
                // check each point in the index
                GameObject item;
                positions.TryGetValue(point + offset, out item);

                if (item == null) continue;
                else list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Renders a pattern stored in the cache variable
        /// </summary>
        /// <param name="offset">Position offset</param>
        /// <param name="direction">GameObject.Facing</param>
        public void RenderCachedPattern(Point offset, Data.Direction direction)
        {
            StopRenderPattern();

            if (currentPattern != null)
                RenderPattern(currentPattern, offset, direction);
            else
                Logger.Report(this, "Warning! No pattern loaded in cache!");
        }

        /// <summary>
        /// Does what you think it does
        /// </summary>
        public void StopRenderPattern()
        {
            SecondSurface.Clear();

            IsRenderingPattern = false;
        }

        /// <summary>
        /// Loads a pattern into the cache variable
        /// </summary>
        /// <param name="p">Pattern to load</param>
        public void CachePattern(Pattern p)
        {
            currentPattern = p;
        }

        public void ClearCachePattern()
        {
            currentPattern = null;
        }

        public void Mark(int x, int y)
        {
            Surface.SetForeground(x, y, Color.Red);
            Surface.SetBackground(x, y, Color.Red);
        }
    }
}
