using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    public struct Ray
    {
        public Ray(Vector3 pos, Vector3 dir)
        {
            Position = pos;
            Direction = dir;
        }

        public Vector3 Position;
        public Vector3 Direction;
    }
}
