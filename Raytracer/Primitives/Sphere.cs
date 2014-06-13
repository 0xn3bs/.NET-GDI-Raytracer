using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer.Primitives
{
    public class Sphere : IIntersectable
    {
        public Sphere(Vector3 center, float radius, Color diffuse, bool isLight = false, bool reflect = false)
        {
            Center = center;
            Radius = radius;
            Diffuse = diffuse;
            IsLight = isLight;
            Reflect = reflect;
            Specular = new Color(255, 255, 255);
        }

        public Intersection Intersects(Ray ray)
        {
            var a = ray.Direction.LengthSquared();
            var b = 2.0 * (Vector3.Dot(ray.Direction, ray.Position - Center));
            var c = (ray.Position - Center).LengthSquared() - (Radius * Radius);

            var disc = b * b - 4.0f * a * c;

            if(disc < 0)
            {
                return new Intersection() { Distance = null, Primitive = this }; 
            }

            var sqrt = Math.Sqrt(disc);
            var t0 = (-b + sqrt) / (2.0 * a);

            if(disc == 0)
            {
                return new Intersection() { Distance = (float)t0, Primitive = this };
            }

            var t1 = (-b - sqrt) / (2.0 * a);

            if (t0 > t1)
            {
                double temp = t0;
                t0 = t1;
                t1 = temp;
            }

            return new Intersection() { Distance = (float)t0, Primitive = this }; 
        }

        public Vector3 GetNormal(Vector3 position)
        {
            var dir = position - Center;
            dir.Normalize();
            return dir;
        }

        public Vector3 Center;
        public float Radius { get; set; }
        public Color Diffuse { get; set; }
        public Color Specular { get; set; }
        public bool IsLight { get; set; }
        public bool Reflect { get; set; }
    }
}
