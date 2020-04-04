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
    }
}
