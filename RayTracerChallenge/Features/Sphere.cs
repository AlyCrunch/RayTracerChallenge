using System.Collections.Generic;

namespace RayTracerChallenge.Features
{
    public class Sphere
    {
        public double Radius { get; set; }
        public PointType Center { get; set; }
        public Matrix Transform { get; set; }
        public Material Material { get; set; }

        public Sphere()
        {
            Radius = 1;
            Center = PointType.Point(0, 0, 0);
            Transform = Matrix.GetIdentity(4, 4);
            Material = new Material();
        }

        public Matrix GetTransformed()
            => Matrix.Inverse(Transform);
        
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

        public Intersection[] Intersect(Ray r)
            => Intersection.Intersect(this, r);

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
            hashCode = hashCode * -1521134295 + EqualityComparer<PointType>.Default.GetHashCode(Center);
            hashCode = hashCode * -1521134295 + EqualityComparer<Matrix>.Default.GetHashCode(Transform);
            hashCode = hashCode * -1521134295 + EqualityComparer<Material>.Default.GetHashCode(Material);
            return hashCode;
        }
        #endregion
    }
}