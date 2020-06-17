using System;
using System.Collections.Generic;

namespace RayTracerChallenge.Features.Shapes
{
    public class Cylinder : Shape
    {
        const double EPSILON = 0.00001;

        public double Maximum { get; set; } = double.PositiveInfinity;
        public double Minimum { get; set; } = double.NegativeInfinity;
        public bool Closed { get; set; } = false;

        public Cylinder()
        {
            Transform = Matrix.GetIdentity();
            Material = new Material();
        }

        protected override Intersection[] LocalIntersect(Ray ray)
        {
            if (!Bounds().Intersects(ray)) return new Intersection[] { };

            var xs = new List<Intersection>();
            var a = Math.Pow(ray.Direction.X, 2) + Math.Pow(ray.Direction.Z, 2);

            if (a <= EPSILON)
            {
                xs.AddRange(IntersectCaps(this, ray));
                return xs.ToArray();
            }

            var b = 2 * ray.Origin.X * ray.Direction.X +
                    2 * ray.Origin.Z * ray.Direction.Z;
            var c = Math.Pow(ray.Origin.X,2) + Math.Pow(ray.Origin.Z, 2) - 1;

            var disc = b * b - 4 * a * c;

            if(disc < 0) return new Intersection[] { };

            var t0 = (-b - Math.Sqrt(disc)) / (2 * a);
            var t1 = (-b + Math.Sqrt(disc)) / (2 * a);


            var y0 = ray.Origin.Y + t0 * ray.Direction.Y;
            if (Minimum < y0 && y0 < Maximum)
                xs.Add(new Intersection(t0, this));
            var y1 = ray.Origin.Y + t1 * ray.Direction.Y;
            if (Minimum < y1 && y1 < Maximum)
                xs.Add(new Intersection(t1, this));

            xs.AddRange(IntersectCaps(this, ray));

            return xs.ToArray();
        }

        protected override PointType LocalNormalAt(PointType point, Intersection hit = null)
        {
            var dist = point.X * point.X + point.Z * point.Z;

            if (dist < 1 && point.Y >= Maximum - EPSILON)
                return PointType.Vector(0, 1, 0);

            if (dist < 1 && point.Y <= Minimum + EPSILON)
                return PointType.Vector(0, -1, 0);

            return PointType.Vector(point.X, 0, point.Z);
        }

        private bool CheckCap(Ray ray, double t)
        {
            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;

            return (x * x + z * z) <= 1;
        }

        private List<Intersection> IntersectCaps(Cylinder cyl, Ray ray)
        {
            var xs = new List<Intersection>();

            if (cyl.Closed == false || Math.Abs(ray.Direction.Y) <= EPSILON)
                return xs;

            var t = (cyl.Minimum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, t))
                xs.Add(new Intersection(t, cyl));

            t = (cyl.Maximum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, t))
                xs.Add(new Intersection(t, cyl));

            return xs;
        }

        public override BoundingBox Bounds()
        {
            var min = PointType.Point(-1, Minimum, -1);
            var max = PointType.Point(1, Maximum, 1);

            return new BoundingBox(min, max);
        }

        public override bool Equals(object obj)
        {
            return obj is Cylinder cylinder &&
                   base.Equals(obj) &&
                   Transform == cylinder.Transform &&
                   Material == cylinder.Material &&
                   SavedRay == cylinder.SavedRay &&
                   Parent == cylinder.Parent &&
                   HasParent == cylinder.HasParent &&
                   Maximum == cylinder.Maximum &&
                   Minimum == cylinder.Minimum &&
                   Closed == cylinder.Closed;
        }

        public override int GetHashCode()
        {
            int hashCode = 1481798398;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Transform.GetHashCode();
            hashCode = hashCode * -1521134295 + Material.GetHashCode();
            hashCode = hashCode * -1521134295 + SavedRay.GetHashCode();
            hashCode = hashCode * -1521134295 + Parent.GetHashCode();
            hashCode = hashCode * -1521134295 + HasParent.GetHashCode();
            hashCode = hashCode * -1521134295 + Maximum.GetHashCode();
            hashCode = hashCode * -1521134295 + Minimum.GetHashCode();
            hashCode = hashCode * -1521134295 + Closed.GetHashCode();
            return hashCode;
        }
    }
}