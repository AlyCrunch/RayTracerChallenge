using System;
using System.Collections.Generic;
using pt = RayTracerChallenge.Features.PointType;

namespace RayTracerChallenge.Features.Shapes
{
    public class Plane : Shape
    {
        const double EPSILON = 0.00001;

        protected override Intersection[] LocalIntersect(Ray ray)
        {
            if (!Bounds().Intersects(ray)) return new Intersection[] { };
            if (Math.Abs(ray.Direction.Y) < EPSILON)
                return Array.Empty<Intersection>();

            var t = -ray.Origin.Y / ray.Direction.Y;

            return new Intersection[] { new Intersection(t, this) };
        }
        public Intersection[] LIntersect(Ray r) => LocalIntersect(r);

        protected override PointType LocalNormalAt(PointType localPoint, Intersection hit = null)
            => PointType.Vector(0, 1, 0);
        public PointType LNormalAt(PointType p) => LocalNormalAt(p);

        public override BoundingBox Bounds()
        => new BoundingBox(
            pt.Point(double.NegativeInfinity, 0, double.NegativeInfinity),
            pt.Point(double.PositiveInfinity, 0, double.PositiveInfinity));

        public override bool Equals(object obj)
        {
            return obj is Plane plane &&
                   base.Equals(obj) &&
                   Transform.Equals(plane.Transform) &&
                   Material.Equals(plane.Material) &&
                   SavedRay == plane.SavedRay &&
                   Parent == plane.Parent &&
                   HasParent == plane.HasParent;
        }

        public override int GetHashCode()
        {
            int hashCode = 480802305;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Transform.GetHashCode();
            hashCode = hashCode * -1521134295 + Material.GetHashCode();
            hashCode = hashCode * -1521134295 + SavedRay.GetHashCode();
            hashCode = hashCode * -1521134295 + Parent.GetHashCode();
            hashCode = hashCode * -1521134295 + HasParent.GetHashCode();
            return hashCode;
        }
    }
}