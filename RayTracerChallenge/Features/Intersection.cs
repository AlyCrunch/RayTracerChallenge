using System.Linq;
using RayTracerChallenge.Features.Shapes;

namespace RayTracerChallenge.Features
{
    public class Intersection
    {
        public double T { get; set; }
        public Shape Object { get; set; }

        public Intersection(double t, Shape @object)
        {
            T = t;
            Object = @object;
        }

        public static Intersection[] Intersections(params Intersection[] intersects)
        {
            return intersects;
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
