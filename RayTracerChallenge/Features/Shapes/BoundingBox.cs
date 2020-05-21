using System;
using System.Collections.Generic;
using pt = RayTracerChallenge.Features.PointType;

namespace RayTracerChallenge.Features.Shapes
{
    public class BoundingBox
    {
        const double EPSILON = 0.00001;
        public PointType Minimum { get; set; }
        public PointType Maximum { get; set; }

        public static BoundingBox Infinite
            => new BoundingBox(
                pt.Point(
                    double.NegativeInfinity,
                    double.NegativeInfinity,
                    double.NegativeInfinity),
                pt.Point(
                    double.PositiveInfinity,
                    double.PositiveInfinity,
                    double.PositiveInfinity));
        public static BoundingBox Empty
            => new BoundingBox(
                pt.Point(
                    double.PositiveInfinity,
                    double.PositiveInfinity,
                    double.PositiveInfinity),
                pt.Point(
                    double.NegativeInfinity,
                    double.NegativeInfinity,
                    double.NegativeInfinity));

        public BoundingBox(PointType min, PointType max)
        {
            Minimum = min;
            Maximum = max;
        }

        public BoundingBox() { Minimum = Empty.Minimum; Maximum = Empty.Maximum; }

        #region Operators
        public static BoundingBox operator +(BoundingBox a, BoundingBox b)
        {
            var min = pt.Point(Math.Min(a.Minimum.X, b.Minimum.X),
                Math.Min(a.Minimum.Y, b.Minimum.Y),
                Math.Min(a.Minimum.Z, b.Minimum.Z));

            var max = pt.Point(Math.Max(a.Maximum.X, b.Maximum.X),
                Math.Max(a.Maximum.Y, b.Maximum.Y),
                Math.Max(a.Maximum.Z, b.Maximum.Z));

            return new BoundingBox(min, max);
        }
        public static BoundingBox operator +(BoundingBox a, PointType p)
        {
            var min = pt.Point(Math.Min(a.Minimum.X, p.X),
                Math.Min(a.Minimum.Y, p.Y),
                Math.Min(a.Minimum.Z, p.Z));

            var max = pt.Point(Math.Max(a.Maximum.X, p.X),
                Math.Max(a.Maximum.Y, p.Y),
                Math.Max(a.Maximum.Z, p.Z));

            return new BoundingBox(min, max);
        }
        #endregion

        public void Add(PointType point)
        {
            Minimum.X = Math.Min(Minimum.X, point.X);
            Minimum.Y = Math.Min(Minimum.Y, point.Y);
            Minimum.Z = Math.Min(Minimum.Z, point.Z);

            Maximum.X = Math.Max(Maximum.X, point.X);
            Maximum.Y = Math.Max(Maximum.Y, point.Y);
            Maximum.Z = Math.Max(Maximum.Z, point.Z);
        }

        public List<PointType> GetCorners()
        {
            return new List<PointType>()
            {
                pt.Point(Minimum.X,Minimum.Y,Minimum.Z),
                pt.Point(Minimum.X,Minimum.Y,Maximum.Z),
                pt.Point(Minimum.X,Maximum.Y,Minimum.Z),
                pt.Point(Minimum.X,Maximum.Y,Maximum.Z),
                pt.Point(Maximum.X,Minimum.Y,Minimum.Z),
                pt.Point(Maximum.X,Minimum.Y,Maximum.Z),
                pt.Point(Maximum.X,Maximum.Y,Minimum.Z),
                pt.Point(Maximum.X,Maximum.Y,Maximum.Z)
            };
        }

        public BoundingBox Transform(Matrix transform)
        {
            var bbox = Empty;
            foreach (var point in GetCorners())
                bbox += transform * point;

            return bbox;
        }

        public void Add(BoundingBox box)
        {
            Add(box.Minimum);
            Add(box.Maximum);
        }

        public bool ContainsPoint(pt point)
        {
            return Minimum.X <= point.X && point.X <= Maximum.X &&
                   Minimum.Y <= point.Y && point.Y <= Maximum.Y &&
                   Minimum.Z <= point.Z && point.Z <= Maximum.Z;
        }

        public bool ContainsBox(BoundingBox box)
            => ContainsPoint(box.Minimum) && ContainsPoint(box.Maximum);

        public bool Intersects(Ray r)
        {
            (var xtmin, var xtmax) = CheckAxis(r.Origin.X, r.Direction.X, Maximum.X, Minimum.X);
            (var ytmin, var ytmax) = CheckAxis(r.Origin.Y, r.Direction.Y, Maximum.Y, Minimum.Y);
            (var ztmin, var ztmax) = CheckAxis(r.Origin.Z, r.Direction.Z, Maximum.Z, Minimum.Z);

            var tmin = Shape.Max(xtmin, ytmin, ztmin);
            var tmax = Shape.Min(xtmax, ytmax, ztmax);

            if (tmin > tmax) return false;

            return true;
        }

        private Tuple<double, double> CheckAxis(double origin, double direction, double min, double max)
        {
            var tMinNumerator = (min - origin);
            var tMaxNumerator = (max - origin);

            double tmax, tmin;
            if (Math.Abs(direction) >= EPSILON)
            {
                tmin = tMinNumerator / direction;
                tmax = tMaxNumerator / direction;
            }
            else
            {
                tmin = tMinNumerator * double.PositiveInfinity;
                tmax = tMaxNumerator * double.PositiveInfinity;
            }
            if (tmin > tmax) return new Tuple<double, double>(tmax, tmin);
            return new Tuple<double, double>(tmin, tmax);
        }

        public Tuple<BoundingBox, BoundingBox> Split()
        {
            (var dx, var dy, var dz) = Size();

            var greatest = Shape.Max(dx, dy, dz);

            (var x0, var y0, var z0, _) = Minimum.ToTuple();
            (var x1, var y1, var z1, _) = Maximum.ToTuple();

            if (greatest == dx)
                x0 = x1 = x0 + dx / 2.0;
            else if (greatest == dy)
                y0 = y1 = y0 + dy / 2.0;
            else
                z0 = z1 = z0 + dz / 2.0;

            var midMin = pt.Point(x0, y0, z0);
            var midMax = pt.Point(x1, y1, z1);

            var left = new BoundingBox(Minimum, midMax);
            var right = new BoundingBox(midMin, Maximum);

            return new Tuple<BoundingBox, BoundingBox>(left, right);
        }

        private Tuple<double, double, double> Size()
        {
            return new Tuple<double, double, double>(Maximum.X - Minimum.X,
                            Maximum.Y - Minimum.Y,
                            Maximum.Z - Minimum.Z);
        }
    }
}
