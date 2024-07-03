using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Logging;

namespace VEngine.Data
{
    /// <summary>
    /// Used for variable stats like HP, MP, SP
    /// </summary>
    public class Stat : IComparer<Stat>
    {
        private int _current;
        private int _max;

        /// <summary>
        /// If true, allows current to go over the max.
        /// </summary>
        public bool IsOverloadable { get; set; } = false;

        public bool Overloaded 
        {
            get => _current > _max;
        }
        public bool ResetCurrent
        {
            set
            {
                if (value) _current = _max;
            }
        }
        public int Current
        {
            get => _current;
            set => _current = IsOverloadable ? _current = value : Math.Min(value, _max);
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

        public static implicit operator Stat(int val)
            => new Stat(val);

        public static bool operator <(Stat lhs, Stat rhs)
            => lhs.Current < rhs.Current;

        public static bool operator >(Stat lhs, Stat rhs)
            => lhs.Current > rhs.Current;

        public static bool operator >=(Stat lhs, Stat rhs)
            => lhs.Current >= rhs.Current;

        public static bool operator <=(Stat lhs, Stat rhs)
            => lhs.Current <= rhs.Current;

        public static bool operator <(Stat lhs, int rhs)
            => lhs.Current < rhs;
        
        public static bool operator >(Stat lhs, int rhs)
            => lhs.Current > rhs;
        
        public static bool operator <=(Stat lhs, int rhs)
            => lhs.Current <= rhs;

        public static bool operator >=(Stat lhs, int rhs)
            => lhs.Current >= rhs;

        public static Stat operator ++(Stat stat)
        {
            stat.Current++;
            return stat;
        }

        public static Stat operator --(Stat stat)
        {
            stat.Current--;
            return stat;
        }

        public static Stat operator +(Stat stat, int value)
        {
            return new Stat(stat.Current + value, stat.Max);
        }

        public static Stat operator -(Stat stat, int value)
        {
            return new Stat(stat.Current - value, stat.Max);
        }

        public static Stat operator *(Stat stat, int value)
        {
            return new(stat.Current * value, stat.Max);
        }

        public static implicit operator int(Stat stat)
        {
            return stat.Current;
        }

        /// <summary>
        /// Warning! Uses integer division.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Stat operator /(Stat stat, int value) 
        {
            return new(stat.Current / value, stat.Max);
        }


        public int Compare(Stat x, Stat y)
        {
            return x.Current - y.Current;
        }

        public override string ToString()
        {
            return _current.ToString();
        }
    }
}
