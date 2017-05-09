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
                Simplex.Noise.Seed = random.Next();
            }
            return random;
        }

        public static float GetFloat(float min, float max)
        {
            float range = max - min;
            float value = range * (float)GetInstance().NextDouble() + min;
            return value;
        }
    }
}
