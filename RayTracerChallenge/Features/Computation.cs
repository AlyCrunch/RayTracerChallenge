using RTH = RayTracerChallenge.Helpers;
using shapes = RayTracerChallenge.Features.Shapes;

namespace RayTracerChallenge.Features
{
    public class Computation
    {
        public double T { get; set; }
        public shapes.Shape Object { get; set; }
        public PointType Point { get; set; }
        public PointType OverPoint { get; set; }
        public PointType EyeV { get; set; }
        public PointType NormalV { get; set; }
        public bool Inside { get; set; }

        public static Computation PrepareComputation(Intersection i, Ray r)
        {
            var c = new Computation
            {
                T = i.T,
                Object = i.Object
            };

            c.Point = RTH.Transformations.Position(r, c.T);
            c.EyeV = -r.Direction;
            c.NormalV = (c.Object as shapes.Shape).NormalAt(c.Point);

            if (PointType.DotProduct(c.NormalV, c.EyeV) < 0)
            {
                c.Inside = true;
                c.NormalV = -c.NormalV;
            }
            else
                c.Inside = false;

            c.OverPoint = c.Point + c.NormalV * 0.0001;

            return c;
        }
    }
}