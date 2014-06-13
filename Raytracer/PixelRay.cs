using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Raytracer
{
    public class PixelRay
    {
        public PixelRay(Point pixelCoordinates, Vector3 position, Vector3 nonNormalizedDirection, bool isSubray = false)
        {
            _Rays = new List<Ray>();
            _PixelCoords = pixelCoordinates;

            if (isSubray)
            {
                var d = nonNormalizedDirection;
                d.Normalize();
                _Rays.Add(new Ray(position, d));
                return;
            }
            else
                if (Scene.SamplesIsPerfectSquare)
                {
                    for (int i = 0; i < Scene.Samples_rt2; ++i)
                    {
                        for (int j = 0; j < Scene.Samples_rt2; ++j)
                        {
                            var x = 1.0f / Scene.Samples + (float)i / Scene.Samples_rt2;
                            var y = 1.0f / Scene.Samples + (float)j / Scene.Samples_rt2;
                            var d = nonNormalizedDirection + new Vector3(x, y, 0);
                            d.Normalize();
                            _Rays.Add(new Ray(position, d));
                        }
                    }
                }
                else
                {
                    var d = nonNormalizedDirection + new Vector3(0.5f, 0.5f, 0.0f);
                    d.Normalize();
                    _Rays.Add(new Ray(position, d));
                    return;
                }
        }

        public Color GetColor()
        {
            int r = 0;
            int g = 0;
            int b = 0;

            var totalRays = _Rays.Count;

            for (int i = 0; i < _Rays.Count; ++i)
            {
                var ray = _Rays.ElementAt(i);

                List<Intersection> intersections = null;

                    intersections = (from obj in Scene.Objects
                        let Ints = obj.Intersects(ray)
                        where Ints.Distance.HasValue && Ints.Distance.Value > 0
                        orderby Ints.Distance ascending
                        select Ints).ToList();

                if (intersections.Count > 0)
                {
                    if (intersections[0].Primitive.IsLight)
                    {
                        r += intersections[0].Primitive.Diffuse.R;
                        g += intersections[0].Primitive.Diffuse.G;
                        b += intersections[0].Primitive.Diffuse.B;
                        continue;
                    }

                    var distance = intersections[0].Distance.Value;
                    var intersection_position = ray.Position + ray.Direction * distance;

                    if (intersections[0].Primitive.Reflect == true)
                    {
                        var rayI = _Rays.ElementAt(i);
                        rayI.Position = intersection_position;
                        rayI.Direction = Vector3.Reflect(rayI.Direction, intersections[0].Primitive.GetNormal(intersection_position));
                        _Rays[i] = rayI;
                        --i;
                        continue;
                    }

                    foreach (var light in Scene.Lights)
                    {
                        var vecToLight = light.Position - intersection_position;
                        var directionToLight = vecToLight;
                        directionToLight.Normalize();

                        var shadowRay = new Ray(intersection_position, directionToLight);

                        var lightRayMag = vecToLight.Length();

                        List<Intersection> shadowRayIntersections = null;

                        shadowRayIntersections = (from obj in Scene.Objects
                            let Ints = obj.Intersects(shadowRay)
                            where Ints.Distance.HasValue && 
                                  Ints.Distance.Value > 0 && 
                                  Ints.Distance < lightRayMag && 
                                  Ints.Primitive != intersections[0].Primitive
                            orderby Ints.Distance ascending
                            select Ints).ToList();

                        var shadowValue = shadowRayIntersections.Count + 1;

                        var surfaceNormal = intersections[0].Primitive.GetNormal(intersection_position);

                        float lightIntensity = Vector3.Dot(directionToLight, surfaceNormal) * (1.0f / (vecToLight.LengthSquared()));

                        if (lightIntensity > 0)
                        {
                            r += Convert.ToInt32(intersections[0].Primitive.Diffuse.R * lightIntensity / shadowValue);
                            g += Convert.ToInt32(intersections[0].Primitive.Diffuse.G * lightIntensity / shadowValue);
                            b += Convert.ToInt32(intersections[0].Primitive.Diffuse.B * lightIntensity / shadowValue);
                        }
                    }
                }
                else
                {
                    var sunsetColor = new Color(250, 214, 165);
                    var skyBlue = new Color(135, 206, 235);

                    var lerpR = (int)MathHelper.Lerp((float)sunsetColor.R, (float)skyBlue.R, ray.Direction.Y);
                    var lerpG = (int)MathHelper.Lerp((float)sunsetColor.G, (float)skyBlue.G, ray.Direction.Y);
                    var lerpB = (int)MathHelper.Lerp((float)sunsetColor.B, (float)skyBlue.B, ray.Direction.Y);
                    r += lerpR;
                    g += lerpG;
                    b += lerpB;
                }
            }

            r /= _Rays.Count;
            g /= _Rays.Count;
            b /= _Rays.Count;

            r = (int)MathHelper.Clamp(r, 0, 255);
            g = (int)MathHelper.Clamp(g, 0, 255);
            b = (int)MathHelper.Clamp(b, 0, 255);

            return new Color(r, g, b);
        }

        public int PixelX
        {
            get { return _PixelCoords.X; }
        }

        public int PixelY
        {
            get { return _PixelCoords.Y; }
        }

        private List<Ray> _Rays;
        private Point _PixelCoords;
    }
}
