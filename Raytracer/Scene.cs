using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raytracer.Lights;
using System.Collections.Concurrent;

namespace Raytracer
{
    static class Scene
    {
        public static BlockingCollection<IIntersectable> Objects = new BlockingCollection<IIntersectable>();
        public static BlockingCollection<PixelRay> Rays = new BlockingCollection<PixelRay>();
        public static BlockingCollection<Lights.PointLight> Lights = new BlockingCollection<Lights.PointLight>();
        public static Random Random = new Random();

        public static int Samples = 16;
        public static int Samples_rt2 = ((int)Math.Sqrt(Samples));

        public static bool SamplesIsPerfectSquare = Samples_rt2 % 1 == 0 && Samples != 1;
    }
}
