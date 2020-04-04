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

        public Intersection[] Intersect(Ray r)
            => Intersection.Intersect(this, r);
    }
}