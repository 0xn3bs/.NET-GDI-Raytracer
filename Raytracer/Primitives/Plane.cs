using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer.Primitives
{
    public class Plane : IIntersectable
    {
        public Plane(Vector3 normal, float distance, Color color)
        {
            Normal = normal;
            Distance = distance;
            Diffuse = color;
            Specular = new Color(255, 255, 255);
        }

        public Intersection Intersects(Ray ray)
        {
            var d = Vector3.Dot((Normal * Distance - ray.Position), Normal) / Vector3.Dot(ray.Direction, Normal);
            return new Intersection() { Distance = d, Primitive = this };
        }

        public Vector3 GetNormal(Vector3 position)
        {
            return Normal;
        }

        public Color Diffuse { get; set; }
        public Color Specular { get; set; }
        public bool IsLight { get; set; }
        public bool Reflect { get; set; }
        public float Distance { get; set; }
        public Vector3 Normal { get; set; }
    }
}
