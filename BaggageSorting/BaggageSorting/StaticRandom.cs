using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSorting
{
    public static class StaticRandom
    {
        static int seed = Environment.TickCount;

        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static int Rand()
        {
            return random.Value.Next();
        }

        public static int Rand(int start, int end)
        {
            return random.Value.Next(start, end);
        }

        public static int Rand(int max)
        {
            return random.Value.Next(max);
        }
    }
}
