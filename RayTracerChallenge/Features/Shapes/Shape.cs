using System;
using System.Collections.Generic;

namespace RayTracerChallenge.Features.Shapes
{
    abstract public class Shape
    {
        public Matrix Transform { get; set; }
        public Material Material { get; set; }
        public Ray SavedRay { get; set; }
        public Shape Parent { get; set; } = null;
        public bool HasParent => Parent != null;

        public Shape()
        {
            Transform = Matrix.GetIdentity(4, 4);
            Material = new Material();
        }

        public Matrix GetTransformed()
            => Matrix.Inverse(Transform);

        public Shape(Material material)
        {
            Material = material;
            Transform = Matrix.GetIdentity(4, 4);
        }

        public Shape(Matrix transform)
        {
            Transform = transform;
            Material = new Material();
        }

        public Intersection[] Intersect(Ray ray)
        {
            var localRay = Ray.Transform(ray, GetTransformed());
            return LocalIntersect(localRay);
        }

        abstract protected Intersection[] LocalIntersect(Ray localRay);

        public PointType NormalAt(PointType point, Intersection hit = null)
        {
            var localPoint = WorldToObject(point);
            var localNormal = LocalNormalAt(localPoint, hit);
            return NormalToWorld(localNormal);
        }

        abstract protected PointType LocalNormalAt(PointType localPoint, Intersection hit = null);

        abstract public BoundingBox Bounds();
        public BoundingBox ParentSpaceBounds()
            => Bounds().Transform(Transform);

        public virtual bool Includes(Shape obj) => obj == this;

        public static double Max(double a, double b, double c)
            => Math.Max(a, Math.Max(b, c));
        public static double Max(params double[] values)
        {
            var max = double.NegativeInfinity;
            foreach(var val in values)
            {
                max = Math.Max(max, val);
            }
            return max;
        }
        public static double Min(double a, double b, double c)
            => Math.Min(a, Math.Min(b, c));
        public static double Min(params double[] values)
        {
            var min = double.PositiveInfinity;
            foreach (var val in values)
            {
                min = Math.Min(min, val);
            }
            return min;
        }
        public static PointType Min(PointType a, PointType b, PointType c)
        {
            return PointType.Point(Min(a.X, b.X, c.X), Min(a.Y, b.Y, c.Y), Min(a.Z, b.Z, c.Z));
        }
        public static PointType Max(PointType a, PointType b, PointType c)
        {
            return PointType.Point(Max(a.X, b.X, c.X), Max(a.Y, b.Y, c.Y), Max(a.Z, b.Z, c.Z));
        }

        public PointType WorldToObject(PointType point)
        {
            if (HasParent)
                point = Parent.WorldToObject(point);

            return Transform.Inverse() * point;
        }
        public PointType NormalToWorld(PointType normal)
        {
            normal = Transform.Inverse().Transpose() * normal;
            normal.W = 0;
            normal = normal.Normalize();

            if (HasParent)
                normal = Parent.NormalToWorld(normal);

            return normal;
        }

        public override bool Equals(object obj)
        {
            return obj is Shape shape &&
                   Transform.Equals(shape.Transform) &&
                   Material.Equals(shape.Material) &&
                   SavedRay == shape.SavedRay &&
                   Parent == shape.Parent &&
                   HasParent == shape.HasParent;
        }
        public override int GetHashCode()
        {
            int hashCode = 1533363371; 
            hashCode = hashCode * -1521134295 + Transform.GetHashCode();
            hashCode = hashCode * -1521134295 + Material.GetHashCode();
            hashCode = hashCode * -1521134295 + SavedRay.GetHashCode();
            hashCode = hashCode * -1521134295 + Parent.GetHashCode();
            hashCode = hashCode * -1521134295 + HasParent.GetHashCode();
            return hashCode;
        }
    }
}