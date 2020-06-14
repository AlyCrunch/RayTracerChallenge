using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Features.Shapes
{
    public enum Operation
    {
        Union,
        Intersection,
        Difference
    }

    public class CSG : Shape
    {
        public Shape Left { get; set; }
        public Shape Right { get; set; }
        public Operation Operation { get; set; }

        public CSG(Operation op, Shape left, Shape right)
        {
            Operation = op;
            left.Parent = this;
            Left = left;
            right.Parent = this;
            Right = right;
        }

        public static bool IntersectionAllowed(Operation op, bool lhit, bool inl, bool inr)
        {
            if (op == Operation.Union)
                return (lhit && !inr) || (!lhit && !inl);
            else if (op == Operation.Intersection)
                return (lhit && inr) || (!lhit && inl);
            else if (op == Operation.Difference)
                return (lhit && !inr) || (!lhit && inl);
            return false;
        }

        public override BoundingBox Bounds()
        => Left.ParentSpaceBounds() + Right.ParentSpaceBounds();

        protected override Intersection[] LocalIntersect(Ray ray)
        {
            if (!Bounds().Intersects(ray)) return new Intersection[] { };

            var xs = new List<Intersection>();
            xs.AddRange(Left.Intersect(ray));
            xs.AddRange(Right.Intersect(ray));
            xs = xs.OrderBy(x => x.T).ToList();

            return FilterIntersections(xs.OrderBy(x => x.T)).ToArray();
        }

        protected override PointType LocalNormalAt(PointType localPoint, Intersection hit = null)
        {
            throw new NotImplementedException();
        }

        public List<Intersection> FilterIntersections(IEnumerable<Intersection> xs)
        {
            var inl = false;
            var inr = false;

            var result = new List<Intersection>();

            foreach (var i in xs)
            {
                var lhit = Left.Includes(i.Object);
                if (IntersectionAllowed(Operation, lhit, inl, inr))
                    result.Add(i);

                if (lhit)
                    inl = !inl;
                else
                    inr = !inr;
            }

            return result;
        }

        public override bool Includes(Shape obj)
            => Left.Includes(obj) || Right.Includes(obj);
    }
}
