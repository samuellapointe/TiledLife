using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife
{
    static class RandomSingleton
    {
        private static Random random;

        public static Random GetRandom()
        {
            if (random == null)
            {
                random = new Random(1);
            }
            return random;
        }
    }
}
