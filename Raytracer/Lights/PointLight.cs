using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer.Lights
{
    class PointLight
    {
        public PointLight(Vector3 position, Color color, float intensity = 1.0f)
        {
            Position = position;
            Color = color;
            Intensity = intensity;
        }

        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public float Intensity { get; set; }
    }
}
