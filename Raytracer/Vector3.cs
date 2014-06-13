using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    public struct Vector3
    {
        public Vector3 (float val)
        {
            X = val;
            Y = val;
            Z = val;
        }
        public Vector3 (float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static readonly Vector3 Forward = new Vector3(0.0f, 0.0f, 1.0f);
        public static readonly Vector3 Backward = new Vector3(-1.0f, 0.0f, 0.0f);
        public static readonly Vector3 Left = new Vector3(-1.0f, 0.0f, 0.0f);
        public static readonly Vector3 Right = new Vector3(1.0f, 0.0f, 0.0f);
        public static readonly Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f);
        public static readonly Vector3 Down = new Vector3(0.0f, -1.0f, 0.0f);
        public static readonly Vector3 Zero = new Vector3(0.0f, 0.0f, 0.0f);

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Vector3))
                return false;

            var vec = (Vector3)obj;

            return (X == vec.X) && (Y == vec.Y) && (Z == vec.Z);
        }

        public static float Dot(Vector3 vec1, Vector3 vec2)
        {
            float dot1 = vec1.X * vec2.X;
            float dot2 = vec1.Y * vec2.Y;
            float dot3 = vec1.Z * vec2.Z;
            return dot1 + dot2 + dot3;
        }

        public float LengthSquared()
        {
            float dot1 = X * X;
            float dot2 = Y * Y;
            float dot3 = Z * Z;
            return dot1 + dot2 + dot3;
        }

        public static float Length(Vector3 vec)
        {
            float dot1 = vec.X * vec.X;
            float dot2 = vec.Y * vec.Y;
            float dot3 = vec.Z * vec.Z;
            float dot = dot1 + dot2 + dot3;
            return (float)Math.Sqrt(dot);
        }

        public float Length()
        {
            float dot1 = X * X;
            float dot2 = Y * Y;
            float dot3 = Z * Z;
            float dot = dot1 + dot2 + dot3;
            return (float)Math.Sqrt(dot);
        }

        public Vector3 Cross(Vector3 vec)
        {
            float cx = Y * vec.Z - Z * vec.Y;
            float cy = Z * vec.X - X * vec.Z;
            float cz = X * vec.Y - Y * vec.X;
            return new Vector3(cx, cy, cz);
        }

        public void Normalize()
        {
            float dot1 = this.X * this.X;
            float dot2 = this.Y * this.Y;
            float dot3 = this.Z * this.Z;
            float dot = dot1 + dot2 + dot3;

            float length = (float)Math.Sqrt(dot);

            X = X / length;
            Y = Y / length;
            Z = Z / length;
        }

        public static Vector3 Reflect(Vector3 I, Vector3 N)
        {
            return I - 2 * N * Vector3.Dot(I, N);
        }

        public static Vector3 operator +(Vector3 c1, Vector3 c2)
        {
            return new Vector3(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z);
        }

        public static Vector3 operator -(Vector3 c1, Vector3 c2)
        {
            return new Vector3(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z);
        }

        public static Vector3 operator *(Vector3 v1, float s)
        {
            return new Vector3(v1.X * s, v1.Y * s, v1.Z * s);
        }

        public static Vector3 operator *(float s, Vector3 v1)
        {
            return new Vector3(v1.X * s, v1.Y * s, v1.Z * s);
        }

        public float X;
        public float Y;
        public float Z;
    }
}
