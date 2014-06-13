using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    public interface IIntersectable
    {
        Intersection Intersects(Ray ray);
        Vector3 GetNormal(Vector3 position);

        Color Diffuse { get; set; }
        Color Specular { get; set; }

        bool IsLight { get; set; }
        bool Reflect { get; set; }
    }
}
