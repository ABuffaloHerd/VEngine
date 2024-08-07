﻿using SadConsole.Components;
using SadConsole.Effects;
using SadConsole.Entities;
using SadRogue.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.AI.Pathfinder;
using VEngine.Animations;
using VEngine.Data;
using VEngine.Logging;
using VEngine.Objects;
using VEngine.Rendering;

namespace VEngine.Scenes.Combat
{
    /// <summary>
    /// This class handles everything to do with rendering entities and checking collision. It knows what's where
    /// Don't use this outside of combat scenes.
    /// </summary>
    public partial class Arena : ShakingCellSurface
    {
        private const int defaultSize = 12;
        private readonly Point defaultPosition = new (32, 4); // todo: make this initializable from constructor or otherwise.
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

        /// <summary>
        /// Creates a new arena with specified width and height. Automatically scales and positions itself.<br></br>
        /// Sizes work best with multiples of 4 larger than 24, but the min is 16 before it totally breaks down.
        /// </summary>
        /// <param name="width">Obese</param>
        /// <param name="height">Lanky</param>
        /// <exception cref="ArgumentException">Arena can't be tall. Yet.</exception>
        internal Arena(int width, int height) : base(width, height)
        {
            if (height > width) throw new ArgumentException("height cannot be larger than width. If you want a tall arena, fill it with walls.");

            /// === DYNAMIC RESIZING === ///
            float scaleFactor = 64f / width; // 64 is the default size.
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
            int verticalOffset = (int)(scaleFactor / defaultPosition.Y) + scaledDynamicOffset;

            // Calculate the new Y position
            int newYPosition = (int)Math.Floor(defaultPosition.Y / scaleFactor) + verticalOffset;

            // calculate the position based on the arena's width
            Point newPosition = new Point(
                (int)Math.Floor(defaultPosition.X / scaleFactor),
                newYPosition
            );
            Position = newPosition;

            /// === Actual constructor things ===
            EntityManager = new();
            SadComponents.Add(EntityManager);
            magicCircles = new();
            positions = new();

            // Set the animation presets global fontsize to this font size so glyphs are the same size
            AnimationPresets.FontSize = this.FontSize;

            // Run second surface setup
            Logger.Report(this, $"parameter width is {width}");
            Logger.Report(this, $"parameter height is {height}");
            SetupRenderer();
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

        public List<GameObject> GetEntities(Alignment alignment)
        {
            List<GameObject> entities = new();

            foreach(GameObject obj in EntityManager)
            {
                if (obj.Alignment == alignment)
                    entities.Add(obj);
            }

            return entities;
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

        public int CountMagicCircle()
        {
            return magicCircles.Count;
        }

        public bool IsTileFree(Point pos, bool mCircles = false)
        {
            if (pos.X > Width - 1 || pos.Y > Height - 1) return false;
            if (pos.X < 0 || pos.Y < 0) return false;

            if(mCircles) // check magic circles
            {
                if (magicCircles.ContainsKey(pos)) return false;
            }

            //Logger.Report(this, $"THE TILE {pos} CONTAINS {At(pos)}");
            return !positions.ContainsKey(pos);
        }

        public bool IsWithinBounds(Point pos)
        {
            if (pos.X > Width - 1 || pos.Y > Height - 1) return false;
            if (pos.X < 0 || pos.Y < 0) return false;

            return true;
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
        /// Adds an effect. Caller is responsible for setting position.
        /// </summary>
        /// <param name="effect"></param>
        public void PlayAnimatedEffect(AnimatedEffect effect)
        {
            Children.Add(effect);
            effect.Position += (1, 1); // offset it. I don't know why but this fixes all positioning issues.
            effect.Start();
        }

        public GameObject? At(int x, int y)
        {
            GameObject? a;
            positions.TryGetValue((x, y), out a);

            return a;
        }

        public GameObject? At(Point p)
        {
            return At(p.X, p.Y);
        }

        /// <summary>
        /// Converts arena into bitboard so that AI knows where not to go.
        /// </summary>
        /// <returns></returns>
        public BitArray ToBitBoard()
        {
            BitArray array = new(this.Width * this.Height);

            foreach(var kvp in positions)
            {
                array.Set(kvp.Key.Y * Width + kvp.Key.X, true);
            }

            return array;
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
