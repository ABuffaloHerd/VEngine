using SadConsole.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using Components = SadConsole.Components;

namespace VEngine.Animations
{
    public class AnimatedEffect : AnimatedScreenObject, ICloneable
    {
        private SadConsole.Components.Timer timer;
        private Point direction;
        private TimeSpan interval;
        private TimeSpan elapsed = TimeSpan.Zero;

        private DateTime start;

        public Point Destination { get; set; }

        /// <summary>
        /// Automatically moving animted screen object
        /// </summary>
        /// <inheritdoc></inheritdoc>
        public AnimatedEffect(string name, int width, int height, TimeSpan moveInterval, Point direction) : base(name, width, height)
        {
            this.direction = direction;
            this.interval = moveInterval;

            timer = new(moveInterval);
            SadComponents.Add(timer);

            timer.TimerElapsed += MoveMe;
        }

        private void MoveMe(object? sender, EventArgs e)
        {
            Position += direction;
            elapsed += DateTime.Now - start;

            if (Position == Destination)
            {
                State = AnimationState.Finished;
                StopTimer();
            }

            if(elapsed > TimeSpan.FromMinutes(1) && Destination == Point.None)
            {
                State = AnimationState.Finished;
                StopTimer();
            }
        }

        public void StartTimer()
        {
            timer.Start();
            start = DateTime.Now;
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public object Clone()
        {
            return new AnimatedEffect(
                Name, Width, Height,
                interval, direction)
            {
                Destination = this.Destination
            };
        }
    }
}
