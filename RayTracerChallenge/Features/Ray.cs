using System.Collections.Generic;

namespace RayTracerChallenge.Features
{
    public class Ray
    {
        public PointType Origin { get; set; }
        public PointType Direction { get; set; }

        public Ray(PointType origin, PointType direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public static Ray Transform(Ray r, Matrix m)
        {
            return new Ray(m * r.Origin, m * r.Direction);
        }

        public override bool Equals(object obj)
        {
            return obj is Ray ray &&
                   Origin.Equals(ray.Origin) &&
                   Direction.Equals(ray.Direction);
        }

        public override int GetHashCode()
        {
            int hashCode = -1708057391;
            hashCode = hashCode * -1521134295 + Origin.GetHashCode();
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            return hashCode;
        }
    }
}
