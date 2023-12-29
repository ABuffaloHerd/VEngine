﻿using SadConsole.Components;
using SadConsole.Effects;
using SadConsole.Entities;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Animations;
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
        private const int defaultSize = 12;
        private readonly Point defaultPosition = new (32, 4);
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
        private Dictionary<Point, MagicCircle> magicCircles;
        private Pattern? currentPattern; // cache the current pattern

        internal Arena(int width, int height) : base(width, height)
        {
            if (height > width) throw new ArgumentException("height cannot be larger than width. If you want a tall arena, fill it with walls.");

            float scaleFactor = 64f / width;
            int size = (int) Math.Floor(scaleFactor * defaultSize);
            FontSize = (size, size);

            /// When width and height are equal, dynamicOffset becomes zero, making scaledDynamicOffset and consequently 
            /// verticalOffset also zero (ignoring the fixed 4 adjustment).
            /// As height decreases relative to width, dynamicOffset increases, and so does verticalOffset, 
            /// pushing the console upward to keep it vertically centered.

            // Calculate the dynamic part of the vertical offset
            int dynamicOffset = (width - height) / 2;  // The difference between width and height, halved

            // Adjust this dynamic offset by the scale factor
            int scaledDynamicOffset = (int)Math.Floor(dynamicOffset / scaleFactor);

            // Calculate the total vertical offset
            // 4 appears to be a magic number but it's just the default Y position.
            int verticalOffset = (int)(scaleFactor / 4) + scaledDynamicOffset;

            // Calculate the new Y position
            int newYPosition = (int)Math.Floor(defaultPosition.Y / scaleFactor) + verticalOffset;

            // calculate the position based on the arena's width
            Point newPosition = new Point(
                (int)Math.Floor(defaultPosition.X / scaleFactor),
                //(int)Math.Floor(defaultPosition.Y / scaleFactor) + verticalOffset
                newYPosition
            );
            Position = newPosition;

            Resize(width, height, true);

            EntityManager = new();
            SadComponents.Add(EntityManager);
            magicCircles = new();
            positions = new();

            // Set the animation presets global fontsize to this font size so glyphs are the same size
            AnimationPresets.FontSize = this.FontSize;

            AnimatedEffect ef = AnimationPresets.ExplosionEffect(8, TimeSpan.FromSeconds(2));

            ef.Position = (6, 6);
            Children.Add(ef);
            ef.Start();
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

        public void AddMagicCircle(MagicCircle magicCircle)
        {
            EntityManager.Add(magicCircle);

            magicCircles.Add(magicCircle.Position, magicCircle);
        }

        public void RemoveMagicCircle(MagicCircle magicCircle)
        {
            EntityManager.Remove(magicCircle);

            magicCircles.Remove(magicCircle.Position);
        }

        public bool IsTileFree(Point pos, bool mCircles = false)
        {
            Logger.Report(this, $"Checking target {pos.X}, {pos.Y}");
            if (pos.X > Width - 1 || pos.Y > Height - 1) return false;
            if (pos.X < 0 || pos.Y < 0) return false;

            if(mCircles) // check magic circles
            {
                if (magicCircles.ContainsKey(pos)) return false;
            }

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
