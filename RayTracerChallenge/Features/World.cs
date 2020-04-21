using RayTracerChallenge.Helpers;
using System.Collections.Generic;
using RayTracerChallenge.Features.Shapes;
using System.Linq;

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
                    Color.White()),
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

        public static Color ShadeHit(World w, Computation comps)
        {
            var shadowed = Light.IsShadowed(w, comps.OverPoint);
            return Light.Lighting(
                comps.Object.Material,
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
            var inters = w.Intersect(r);
            var hit = Intersection.Hit(inters);

            if (hit == null) return Color.Black();
            var comps = Computation.PrepareComputation(hit, r);
            return ShadeHit(w, comps);
        }

        public Color ColorAt(Ray r)
            => ColorAt(this, r);

        public Intersection[] Intersect(Ray r)
        {
            var its = new List<Intersection>();
            foreach (var obj in Objects)
                its.AddRange(obj.Intersect(r));

            return its.OrderBy(x => x.T).ToArray();
        }
    }
}
