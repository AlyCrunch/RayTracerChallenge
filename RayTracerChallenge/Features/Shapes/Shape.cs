using System;
using System.Drawing;

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

        public PointType NormalAt(PointType point)
        {
            var localPoint = WorldToObject(point);
            var localNormal = LocalNormalAt(localPoint);
            return NormalToWorld(localNormal);
        }

        abstract protected PointType LocalNormalAt(PointType localPoint);

        abstract public BoundingBox Bounds();
        public BoundingBox ParentSpaceBounds()
            => Bounds().Transform(Transform);

        public static double Max(double a, double b, double c)
            => Math.Max(a, Math.Max(b, c));
        public static double Min(double a, double b, double c)
            => Math.Min(a, Math.Min(b, c));

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
    }
}