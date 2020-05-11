using System;
using System.Collections.Generic;

namespace RayTracerChallenge.Features.Shapes
{
    public class Sphere : Shape
    {
        public double Radius { get; set; }
        public PointType Center { get; set; }

        public Sphere()
        {
            Radius = 1;
            Center = PointType.Point(0, 0, 0);
            Transform = Matrix.GetIdentity(4, 4);
            Material = new Material();
        }

        public Sphere(PointType center, double radius)
        {
            Center = center;
            Radius = radius;
            Transform = Matrix.GetIdentity(4, 4);
        }

        public Sphere(Material material)
        {
            Radius = 1;
            Center = PointType.Point(0, 0, 0);
            Material = material;
            Transform = Matrix.GetIdentity(4, 4);
        }

        public Sphere(Matrix transform)
        {
            Radius = 1;
            Center = PointType.Point(0, 0, 0);
            Transform = transform;
            Material = new Material();
        }

        public static Sphere Glass()
        {
            return new Sphere()
            {
                Transform = Matrix.GetIdentity(),
                Material = new Material()
                {
                    Transparency = 1,
                    RefractiveIndex = 1.5
                }
            };
        }

        protected override Intersection[] LocalIntersect(Ray localRay)
        {
            var sphereToRay = localRay.Origin - Center;
            var a = PointType.DotProduct(localRay.Direction, localRay.Direction);
            var b = 2 * PointType.DotProduct(localRay.Direction, sphereToRay);
            var c = PointType.DotProduct(sphereToRay, sphereToRay) - 1;

            var discriminant = Math.Pow(b, 2) - 4 * a * c;

            if (discriminant < 0) return new Intersection[] { };

            var t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            var t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

            return new Intersection[]
            { new Intersection(t1, this), new Intersection(t2, this) };
        }

        protected override PointType LocalNormalAt(PointType localPoint)
        {
            return localPoint - Center;
        }

        #region Overriding
        public override bool Equals(object obj)
        {
            return obj is Sphere sphere &&
                   Radius == sphere.Radius &&
                   Center.Equals(sphere.Center) &&
                   Transform.Equals(sphere.Transform) &&
                   Material.Equals(sphere.Material);
        }

        public override int GetHashCode()
        {
            int hashCode = 805375570;
            hashCode = hashCode * -1521134295 + Radius.GetHashCode();
            hashCode = hashCode * -1521134295 + Center.GetHashCode();
            hashCode = hashCode * -1521134295 + Transform.GetHashCode();
            hashCode = hashCode * -1521134295 + Material.GetHashCode();
            return hashCode;
        }

        #endregion
    }
}