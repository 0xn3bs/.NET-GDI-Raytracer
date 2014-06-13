using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    public struct Color
    {
        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Color(int r, int g, int b)
        {
            R = (byte)r;
            G = (byte)g;
            B = (byte)b;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Color))
                return false;

            var color = (Color)obj;

            return (R == color.R) && (G == color.G) && (B == color.B);
        }

        public static bool operator ==(Color a, Color b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(Color a, Color b)
        {
            return !(a == b);
        }

        public byte R;
        public byte G;
        public byte B;

        public static Color Red = new Color(255, 0, 0);
        public static Color Green = new Color(0, 255, 0);
        public static Color Blue = new Color(0, 0, 255);
        public static Color White = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
        public static Color Gray = new Color(128, 128, 128);
    }
}
