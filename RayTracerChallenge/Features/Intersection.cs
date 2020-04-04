using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracerChallenge.Features
{
    public class Intersection
    {
        public double T { get; set; }
        public object Object { get; set; }

        public Intersection(double t, object @object)
        {
            T = t;
            Object = @object;
        }

        public static Intersection[] Intersections(params Intersection[] intersects)
        {
            return intersects;
        }

        public static Intersection[] Intersect(Sphere s, Ray r)
        {
            r = Ray.Transform(r, Matrix.Inverse(s.Transform));

            var sphereToRay = r.Origin - s.Center;
            var a = PointType.DotProduct(r.Direction, r.Direction);
            var b = 2 * PointType.DotProduct(r.Direction, sphereToRay);
            var c = PointType.DotProduct(sphereToRay, sphereToRay) - 1;

            var discriminant = Math.Pow(b, 2) - 4 * a * c;

            if (discriminant < 0) return new Intersection[] { };

            var t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            var t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

            return new Intersection[] 
            { new Intersection(t1, s), new Intersection(t2, s) };
        }

        public static Intersection[] Intersect(World w, Ray r)
        {
            var its = new List<Intersection>();

            foreach (var obj in w.Objects)
                its.AddRange(Intersect((Sphere)obj, r));

            return its.OrderBy(x => x.T).ToArray();
        }

        public static Intersection Hit(Intersection[] intersections)
        {
            if (!intersections.Any(x => x.T >= 0)) return null;

            return intersections.Where(x => x.T >= 0)
                     .OrderBy(x => x.T)
                     .First();
        }
    }
}
