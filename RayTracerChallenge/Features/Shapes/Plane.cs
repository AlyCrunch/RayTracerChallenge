using System;
using pt = RayTracerChallenge.Features.PointType;

namespace RayTracerChallenge.Features.Shapes
{
    public class Plane : Shape
    {
        const double EPSILON = 0.00001;

        protected override Intersection[] LocalIntersect(Ray localRay)
        {
            if (Math.Abs(localRay.Direction.Y) < EPSILON)
                return Array.Empty<Intersection>();

            var t = -localRay.Origin.Y / localRay.Direction.Y;

            return new Intersection[] { new Intersection(t, this) };
        }
        public Intersection[] LIntersect(Ray r) => LocalIntersect(r);

        protected override PointType LocalNormalAt(PointType localPoint)
            => PointType.Vector(0, 1, 0);
        public PointType LNormalAt(PointType p) => LocalNormalAt(p);

        public override BoundingBox Bounds()
        => new BoundingBox(
            pt.Point(double.NegativeInfinity, 0, double.NegativeInfinity),
            pt.Point(double.PositiveInfinity, 0, double.PositiveInfinity));
    }
}