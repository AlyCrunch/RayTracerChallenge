using RayTracerChallenge.Helpers;
using System.Collections.Generic;
using RayTracerChallenge.Features.Shapes;
using System.Linq;
using System;

namespace RayTracerChallenge.Features
{
    public class World
    {
        public List<Shape> Objects { get; set; } = new List<Shape>();
        public Light Light { get; set; }

        public static World Default()
        {
            return new World
            {
                Light = new Light(
                    PointType.Point(-10, 10, -10),
                    Color.White),
                Objects = new List<Shape>
                {
                    new Sphere(
                            new Material(
                                new Color(0.8, 1, 0.6),
                                0.7,
                                0.2
                            )
                        ),
                    new Sphere(
                            Transformations.Scaling(0.5, 0.5, 0.5)
                            )
                }
            };
        }

        public Color ShadeHit(Computation comps, int remaining = 5)
            => ShadeHit(this, comps, remaining);

        public static Color ShadeHit(World w, Computation comps, int remaining = 5)
        {
            var shadowed = Light.IsShadowed(w, comps.OverPoint);
            var surface = Light.Lighting(
                comps.Object.Material,
                comps.Object,
                w.Light,
                comps.Point,
                comps.EyeV,
                comps.NormalV,
                shadowed);

            var reflected = w.ReflectedColor(comps, remaining);
            var refracted = w.RefractedColor(comps, remaining);

            var material = comps.Object.Material;
            if(material.Reflective > 0 && material.Transparency > 0)
            {
                var reflectance = comps.Schlick();
                return surface
                    + reflected * reflectance
                    + refracted * (1 - reflectance);
            }
            else
                return surface + reflected + refracted;
        }

        public Color ReflectedColor(Computation comps, int remaining = 5)
            => ReflectedColor(this, comps, remaining);

        public static Color ReflectedColor(World w, Computation comps, int remaining = 5)
        {
            if (remaining <= 0)
                return Color.Black;

            if (comps.Object.Material.Reflective == 0)
                return Color.Black;
            var reflectedRay = new Ray(comps.OverPoint, comps.RelflectV);
            var color = w.ColorAt(reflectedRay, remaining - 1);

            return color * comps.Object.Material.Reflective;
        }

        public Color RefractedColor(Computation comps, int remaining = 5)
        {
            if (remaining <= 0) return Color.Black;

            if (comps.Object.Material.Transparency == 0)
                return Color.Black;

            var nRatio = comps.N1 / comps.N2;
            var cosI = PointType.DotProduct(comps.EyeV, comps.NormalV);
            var sin2t = nRatio * nRatio * (1 - cosI * cosI);

            if (sin2t > 1)
            {
                return Color.Black;
            }

            var cosT = Math.Sqrt(1 - sin2t);
            var direction = comps.NormalV * (nRatio * cosI - cosT)
                - comps.EyeV * nRatio;

            var refractRay = new Ray(comps.UnderPoint, direction);

            var color = ColorAt(refractRay, remaining - 1) * 
                comps.Object.Material.Transparency;

            return color;
        }

        public static Color ColorAt(World w, Ray r, int remaining = 5)
        {
            var inters = w.Intersect(r);
            var hit = Intersection.Hit(inters);

            if (hit == null) return Color.Black;
            var comps = Computation.PrepareComputations(hit, r, inters);
            return ShadeHit(w, comps, remaining);
        }

        public Color ColorAt(Ray r, int remaining = 5)
            => ColorAt(this, r, remaining);

        public Intersection[] Intersect(Ray r)
        {
            var its = new List<Intersection>();
            foreach (var obj in Objects)
                its.AddRange(obj.Intersect(r));

            return its.OrderBy(x => x.T).ToArray();
        }
    }
}