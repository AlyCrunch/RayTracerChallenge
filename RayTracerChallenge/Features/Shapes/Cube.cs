using System;
using System.Collections.Generic;
using pt = RayTracerChallenge.Features.PointType;

namespace RayTracerChallenge.Features.Shapes
{
    public class Cube : Shape
    {
        const double EPSILON = 0.00001;

        public Cube(){}
        public Cube(Material m, Matrix t)
        {
            Transform = t;
            Material = m;
        }

        protected override Intersection[] LocalIntersect(Ray ray)
        {
            if (!Bounds().Intersects(ray)) return new Intersection[] { };
            (var xtmin, var xtmax) = CheckAxis(ray.Origin.X, ray.Direction.X);
            (var ytmin, var ytmax) = CheckAxis(ray.Origin.Y, ray.Direction.Y);
            (var ztmin, var ztmax) = CheckAxis(ray.Origin.Z, ray.Direction.Z);

            var tmin = Max(xtmin, ytmin, ztmin);
            var tmax = Min(xtmax, ytmax, ztmax);

            if (tmin > tmax) return new Intersection[] { };

            return new Intersection[] { 
                new Intersection(tmin, this), 
                new Intersection(tmax, this)
            };
        }
        protected override PointType LocalNormalAt(PointType point, Intersection hit = null)
        {
            var maxC = Max(Math.Abs(point.X), Math.Abs(point.Y), Math.Abs(point.Z));

            if(maxC == Math.Abs(point.X))
                return PointType.Vector(point.X, 0, 0);
            if (maxC == Math.Abs(point.Y))
                return PointType.Vector(0, point.Y, 0);
            if (maxC == Math.Abs(point.Z))
                return PointType.Vector(0, 0, point.Z);

            throw new Exception("No normal found");
        }

        private Tuple<double, double> CheckAxis(double origin, double direction)
        {
            var tMinNumerator = (-1 - origin);
            var tMaxNumerator = (1 - origin);
            
            double tmax, tmin;
            if (Math.Abs(direction) >= EPSILON)
            {
                tmin = tMinNumerator / direction;
                tmax = tMaxNumerator / direction;
            }
            else
            {
                tmin = tMinNumerator * double.PositiveInfinity;
                tmax = tMaxNumerator * double.PositiveInfinity;
            }
            if (tmin > tmax) return new Tuple<double, double>(tmax, tmin);
            return new Tuple<double, double>(tmin, tmax);
        }
        
        public override BoundingBox Bounds()
            => new BoundingBox(pt.Point(-1, -1, -1), pt.Point(1, 1, 1));

        public override bool Equals(object obj)
        {
            return obj is Cube cube &&
                   Transform.Equals(cube.Transform) &&
                   Material.Equals(cube.Material) &&
                   SavedRay == cube.SavedRay &&
                   Parent == cube.Parent &&
                   HasParent == cube.HasParent;
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
