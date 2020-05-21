using System;
using System.Collections.Generic;

namespace RayTracerChallenge.Features.Shapes
{
    public class Cone : Shape
    {
        const double EPSILON = 0.00001;
        public double Maximum { get; set; } = double.PositiveInfinity;
        public double Minimum { get; set; } = double.NegativeInfinity;
        public bool Closed { get; set; } = false;

        public Cone()
        {
            Transform = Matrix.GetIdentity();
            Material = new Material();
        }

        protected override Intersection[] LocalIntersect(Ray ray)
        {
            var xs = new List<Intersection>();

            var d = ray.Direction;
            var o = ray.Origin;

            var a = d.X * d.X - d.Y * d.Y + d.Z * d.Z;
            var b = 2 * o.X * d.X - 2 * o.Y * d.Y + 2 * o.Z * d.Z;
            var c = o.X * o.X - o.Y * o.Y + o.Z * o.Z;

            if (Math.Abs(a) <= EPSILON)
            {
                if (Math.Abs(b) <= EPSILON)
                    return xs.ToArray();

                var t = -c / (2 * b);

                if (Math.Abs(t) > EPSILON)
                    xs.Add(new Intersection(t, this));

                xs.AddRange(IntersectCaps(this, ray));
                return xs.ToArray();
            }

            var disc = b * b - 4 * a * c;
            if (disc < 0) return new Intersection[] { };

            var t0 = (-b - Math.Sqrt(disc)) / (2 * a);
            var t1 = (-b + Math.Sqrt(disc)) / (2 * a);

            var y0 = o.Y + t0 * d.Y;
            if (Minimum < y0 && y0 < Maximum)
                xs.Add(new Intersection(t0, this));
            var y1 = o.Y + t1 * d.Y;
            if (Minimum < y1 && y1 < Maximum)
                xs.Add(new Intersection(t1, this));

            xs.AddRange(IntersectCaps(this, ray));

            return xs.ToArray();
        }

        protected override PointType LocalNormalAt(PointType point)
        {
            var dist = point.X * point.X + point.Z * point.Z;

            if (dist < 1 && point.Y >= Maximum - EPSILON)
                return PointType.Vector(0, 1, 0);

            if (dist < 1 && point.Y <= Minimum + EPSILON)
                return PointType.Vector(0, -1, 0);

            var y = Math.Sqrt(dist);
            if (point.Y > 0) 
                y = -y;

            return PointType.Vector(point.X, y, point.Z);
        }

        private bool CheckCap(Ray ray, double t, double limit)
        {
            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;

            return (x * x + z * z) <= Math.Pow(limit, 2);
        }

        private List<Intersection> IntersectCaps(Cone cone, Ray ray)
        {
            var xs = new List<Intersection>();

            if (cone.Closed == false || Math.Abs(ray.Direction.Y) <= EPSILON)
                return xs;

            var t = (cone.Minimum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, t, cone.Minimum))
                xs.Add(new Intersection(t, cone));

            t = (cone.Maximum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, t, cone.Maximum))
                xs.Add(new Intersection(t, cone));

            return xs;
        }

        public override BoundingBox Bounds()
        {
            var r = Math.Max(
                Math.Abs(Minimum),
                Math.Abs(Maximum));

            var min = PointType.Point(-r, Minimum, -r);
            var max = PointType.Point(r, Maximum, r);

            return new BoundingBox(min, max);
        }
    }
}