using System;
using System.Collections.Generic;
using pt = RayTracerChallenge.Features.PointType;

namespace RayTracerChallenge.Features.Shapes
{
    public class Triangle : Shape
    {
        const double EPSILON = 0.00001;

        public pt P1 { get; set; }
        public pt P2 { get; set; }
        public pt P3 { get; set; }
        public pt Edge1 => P2 - P1;
        public pt Edge2 => P3 - P1;
        public pt Normal => pt.CrossProduct(Edge2, Edge1).Normalize();

        //SmoothTriangle
        public pt N1 { get; set; }
        public pt N2 { get; set; }
        public pt N3 { get; set; }
        public bool IsSmoothed { get; set; } = false;

        public Triangle(pt p1, pt p2, pt p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }
        public Triangle(pt p1, pt p2, pt p3, pt n1, pt n2, pt n3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            N1 = n1;
            N2 = n2;
            N3 = n3;
        }
        public static Triangle Smooth(pt p1, pt p2, pt p3, pt n1, pt n2, pt n3)
        {
            return new Triangle(p1, p2, p3)
            {
                N1 = n1,
                N2 = n2,
                N3 = n3,
                IsSmoothed = true
            };
        }

        public override BoundingBox Bounds()
            => new BoundingBox(Min(P1, P2, P3), Max(P1, P2, P3));

        protected override Intersection[] LocalIntersect(Ray ray)
        {
            if (!Bounds().Intersects(ray)) return new Intersection[] { };
            var dirCrossEdge2 = pt.CrossProduct(ray.Direction, Edge2);
            var det = pt.DotProduct(Edge1, dirCrossEdge2);
            if (Math.Abs(det) < EPSILON) return new Intersection[] { };

            var f = 1.0 / det;
            var p1ToOrigin = ray.Origin - P1;
            var u = f * pt.DotProduct(p1ToOrigin, dirCrossEdge2);
            if (u < 0 || u > 1) return new Intersection[] { };

            var originCrossEdge1 = pt.CrossProduct(p1ToOrigin, Edge1);
            var v = f * pt.DotProduct(ray.Direction, originCrossEdge1);
            if(v < 0 || (u+v) > 1) return new Intersection[] { };

            var t = f * pt.DotProduct(Edge2, originCrossEdge1);
            return new Intersection[] { Intersection.WithUV(t, this, u, v) };
        }

        protected override pt LocalNormalAt(pt localPoint, Intersection hit = null)
        {
            if (!IsSmoothed) return Normal;
            return N2 * hit.U +
                   N3 * hit.V +
                   N1 * (1 - hit.U - hit.V);
        }

        public pt TestNormalLocal(pt point)
            => LocalNormalAt(point);

        public override bool Equals(object obj)
        {
            return obj is Triangle triangle &&
                   Transform.Equals(triangle.Transform) &&
                   Material.Equals(triangle.Material) &&
                   Parent == triangle.Parent &&
                   HasParent == triangle.HasParent &&
                   P1.Equals(triangle.P1) &&
                   P2.Equals(triangle.P2) &&
                   P3.Equals(triangle.P3) &&
                   Edge1.Equals(triangle.Edge1) &&
                   Edge2.Equals(triangle.Edge2) &&
                   Normal.Equals(triangle.Normal) &&
                   N1.Equals(triangle.N1) &&
                   N2.Equals(triangle.N2) &&
                   N3.Equals(triangle.N3) &&
                   IsSmoothed == triangle.IsSmoothed;
        }
        public override int GetHashCode()
        {
            int hashCode = -2063967696;
            hashCode = hashCode * -1521134295 + Transform.GetHashCode();
            hashCode = hashCode * -1521134295 + Material.GetHashCode();
            hashCode = hashCode * -1521134295 + SavedRay.GetHashCode();
            hashCode = hashCode * -1521134295 + Parent.GetHashCode();
            hashCode = hashCode * -1521134295 + HasParent.GetHashCode();
            hashCode = hashCode * -1521134295 + P1.GetHashCode();
            hashCode = hashCode * -1521134295 + P2.GetHashCode();
            hashCode = hashCode * -1521134295 + P3.GetHashCode();
            hashCode = hashCode * -1521134295 + Edge1.GetHashCode();
            hashCode = hashCode * -1521134295 + Edge2.GetHashCode();
            hashCode = hashCode * -1521134295 + Normal.GetHashCode();
            hashCode = hashCode * -1521134295 + N1.GetHashCode();
            hashCode = hashCode * -1521134295 + N2.GetHashCode();
            hashCode = hashCode * -1521134295 + N3.GetHashCode();
            hashCode = hashCode * -1521134295 + IsSmoothed.GetHashCode();
            return hashCode;
        }
    }
}
