﻿using System;

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
            return Array.Empty<Intersection>();
        }

        protected override PointType LocalNormalAt(PointType localPoint)
        {
            return PointType.Vector(localPoint.X, localPoint.Y, localPoint.Z);
        }

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
