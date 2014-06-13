using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    public class MathHelper
    {
        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        public static byte Clamp(byte val, byte min, byte max)
        {
            if (val >= max)
                return max;

            if (val <= min)
                return min;

            return val;
        }

        public static int Clamp(int val, int min, int max)
        {
            if (val >= max)
                return max;

            if (val <= min)
                return min;

            return val;
        }
    }
}
