using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleshipsSolution3._0.Algorithms.Helpers
{
    public static class StaticRandom
    {
        static int seed = Environment.TickCount;

        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static int Rand(int val)
        {
            return random.Value.Next(val);
        }
        public static int RandTwo(int val1, int val2)
        {
            return random.Value.Next(val1, val2);
        }
    }
}
