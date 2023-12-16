using SadConsole.Effects;
using SadConsole.Entities;
using SadRogue.Primitives;
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
    /// This class handles everything to do with rendering entities and checking collision.
    /// Don't use this outside of combat scenes.
    /// </summary>
    public class Arena : Console
    {
        public EntityManager EntityManager { get; private set; }

        /// <summary>
        /// Is it rendering a pattern? If set to false stops rendering said pattern.
        /// </summary>
        public bool IsRenderingPattern { get; private set; }

        public bool HasCachedPattern 
        {
            get => currentPattern != null;
        }

        private Dictionary<Point, GameObject> positions;
        private Pattern? currentPattern; // cache the current pattern

        internal Arena(int width, int height) : base(width, height)
        {
            EntityManager = new();

            SadComponents.Add(EntityManager);

            positions = new();
        }

        public void AddEntity(GameObject gameObject)
        {
            EntityManager.Add(gameObject);

            // Rebuild position cache
            UpdatePositions();
        }

        public void RemoveEntity(GameObject gameObject)
        {
            EntityManager.Remove(gameObject); 
            UpdatePositions();
        }

        public bool IsTileFree(Point pos)
        {
            Logger.Report(this, $"Checking target {pos.X}, {pos.Y}");
            if (pos.X > Width || pos.Y > Height) return false;
            if (pos.X < 0 || pos.Y < 0) return false;

            return !positions.ContainsKey(pos);
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
                BlinkOutForegroundColor = Color.Black,
                // Every half a second
                BlinkSpeed = TimeSpan.FromMilliseconds(500),
                RunEffectOnApply = true
            };

            // turn enum direction into a number that the pattern recognises as a valid rotation
            foreach (var point in p.GetRotated((int)direction))
            {
                Point newOffset = point + offset;
                Surface.SetForeground(newOffset.X, newOffset.Y, Color.Yellow);
                Surface.SetGlyph(newOffset.X, newOffset.Y, 'X');
                Surface.SetEffect(newOffset.X, newOffset.Y, b);
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

            foreach(Point point in p.GetRotated((int)direction))
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
            Surface.Clear();

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

        public void UpdatePositions()
        {
            positions = new();

            foreach(GameObject obj in EntityManager)
            {
                if (IsTileFree(obj.Position))
                    positions.Add(obj.Position, obj);
                else
                    Bounce(obj);
            }
        }

        /// <summary>
        /// Updates single entry to prevent O(n) updating
        /// </summary>
        /// <param name="eventArgs"></param>
        public void UpdatePositions(ValueChangedEventArgs<Point> eventArgs)
        {
            positions.TryGetValue(eventArgs.OldValue, out GameObject obj);
            positions.Remove(eventArgs.OldValue);
            positions.Add(eventArgs.NewValue, obj);
        }

        /// <summary>
        /// Normally i'd implement a telefrag but testing takes priority.
        /// This method sends an object to a random position in a 5x5 range around where it shouldn't be
        /// </summary>
        /// <param name="offender">The offending object to be bounced</param>
        private void Bounce(GameObject offender)
        {
            Random rnd = new();
            Point p = (0, 0);
            const int maxAttempts = 25; // Maximum number of attempts to find a free tile
            bool foundFreeTile = false;

            for (int i = 0; i < maxAttempts; i++)
            {
                int x = rnd.Next(-2, 3); // Adjust range for negative offsets
                int y = rnd.Next(-2, 3);
                p = (x, y);

                if (IsTileFree(offender.Position + p))
                {
                    foundFreeTile = true;
                    break;
                }
            }

            if (foundFreeTile)
            {
                offender.Position += p;
            }
            else
            {
                // Handle the case where no free tile is found
                // E.g., keep the offender in the current position or take some other action
                throw new Exception("No valid bounce target and idk what else to do apart from explode");
            }

            UpdatePositions();
        }

    }
}
