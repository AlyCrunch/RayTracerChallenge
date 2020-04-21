namespace RayTracerChallenge.Features.Shapes
{
    abstract public class Shape
    {
        public Matrix Transform { get; set; }
        public Material Material { get; set; }
        public Ray SavedRay { get; set; }

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
            var localPoint = Transform.Inverse() * point;
            var localNormal = LocalNormalAt(localPoint);
            var worldNormal = Matrix.Transpose(Transform.Inverse()) * localNormal;
            worldNormal.W = 0;
            
            return worldNormal.Normalize();
        }

        abstract protected PointType LocalNormalAt(PointType localPoint);
    }
}
