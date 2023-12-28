using SadConsole.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Components = SadConsole.Components;

namespace VEngine.Animations
{
    public class AnimatedEffect : AnimatedScreenObject
    {
        private Components.Timer timer;
        private Point direction;

        /// <summary>
        /// Automatically moving animted screen object
        /// </summary>
        /// <inheritdoc></inheritdoc>
        public AnimatedEffect(string name, int width, int height, TimeSpan moveInterval, Point direction) : base(name, width, height)
        {
            timer = new(moveInterval);
            this.direction = direction;

            timer.Start();
            SadComponents.Add(timer);

            timer.TimerElapsed += MoveMe;
        }

        private void MoveMe(object? sender, EventArgs e)
        {
            this.Position += direction;
        }

        public void StartTimer()
        {
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }
    }
}
