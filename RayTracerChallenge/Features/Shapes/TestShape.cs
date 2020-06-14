using System;
using pt = RayTracerChallenge.Features.PointType;

namespace RayTracerChallenge.Features.Shapes
{
    public class TestShape : Shape
    {
        public TestShape()
        {
            Transform = Matrix.GetIdentity(4, 4);
            Material = new Material();
        }

        public TestShape(Material material)
        {
            Material = material;
            Transform = Matrix.GetIdentity(4, 4);
        }

        public TestShape(Matrix transform)
        {
            Transform = transform;
            Material = new Material();
        }
                
        protected override Intersection[] LocalIntersect(Ray localRay)
        {
            SavedRay = localRay;
            if (!Bounds().Intersects(localRay)) return new Intersection[] { };
            return Array.Empty<Intersection>();
        }

        protected override PointType LocalNormalAt(PointType localPoint, Intersection hit = null)
        {
            return PointType.Vector(localPoint.X, localPoint.Y, localPoint.Z);
        }

        public override BoundingBox Bounds()
            => new BoundingBox(pt.Point(-1, -1, -1), pt.Point(1, 1, 1));

        #region Overriding
        public override bool Equals(object obj)
        {
            return obj is Shape shape &&
                   Transform.Equals(shape.Transform) &&
                   Material.Equals(shape.Material);
        }

        public override int GetHashCode()
        {
            int hashCode = 805375570;
            hashCode = hashCode * -1521134295 + Transform.GetHashCode();
            hashCode = hashCode * -1521134295 + Material.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}
