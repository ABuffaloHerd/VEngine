using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Data
{
    public struct Stat
    {
        private int _current;
        private int _max;
        public int Current
        {
            get => _current;
            set => _current = Math.Min(value, _max);
        }

        public int Max
        {
            get => _max;
            set
            {
                _max = value;
                _current = Math.Min(_current, _max);
            }
        }

        public Stat(int current, int max)
        {
            _current = current;
            _max = max;
        }

        public Stat(int max)
        {
            _max = max;
            _current = max;
        }
    }
}
