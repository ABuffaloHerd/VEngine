using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Scenes.Combat
{
    internal class FightFeed : Console
    {
        private Queue<string> buffer;
        public FightFeed(int width, int height) : base(width, height)
        {
            buffer = new();
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);

            Surface.Clear();

            int y = this.Height;
            foreach(string str in buffer) 
            {
                Surface.Print(0, y--, str);
            }
        }

        public void Print(string str)
        {
            buffer.Enqueue(str);

            if (buffer.Count > Height)
                buffer.Dequeue();
        }
        
    }
}
