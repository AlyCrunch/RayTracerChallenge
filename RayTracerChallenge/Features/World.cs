using RayTracerChallenge.Helpers;
using System.Collections.Generic;
using RayTracerChallenge.Features.Shapes;

namespace RayTracerChallenge.Features
{
    public class World
    {
        public List<object> Objects { get; set; } = new List<object>();
        public Light Light { get; set; }

        public static World Default()
        {
            return new World
            {
                Light = new Light(
                    PointType.Point(-10, 10, -10),
                    Color.White()),
                Objects = new List<object>
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

        public static Color ShadeHit(World w, Computation comps)
        {
            var shadowed = Light.IsShadowed(w, comps.OverPoint);
            return Light.Lighting(
                (comps.Object as Sphere).Material,
                w.Light,
                comps.Point,
                comps.EyeV,
                comps.NormalV, 
                shadowed);
        }

        public Color ShadeHit(Computation comps)
            => ShadeHit(this, comps);

        public static Color ColorAt(World w, Ray r)
        {
            var inters = Intersection.Intersect(w, r);
            var hit = Intersection.Hit(inters);

            if (hit == null) return Color.Black();
            var comps = Computation.PrepareComputation(hit, r);
            return ShadeHit(w, comps);
        }

        public Color ColorAt(Ray r)
            => ColorAt(this, r);
    }
}
