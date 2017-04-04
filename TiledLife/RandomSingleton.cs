using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife
{
    static class RandomGen
    {
        private static Random random;

        public static Random GetInstance()
        {
            if (random == null)
            {
                random = new Random(1);
            }
            return random;
        }
    }
}
