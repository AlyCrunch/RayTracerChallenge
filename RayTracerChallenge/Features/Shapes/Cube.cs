using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Features.Shapes
{
    public class Cube : Shape
    {
        const double EPSILON = 0.00001;

        protected override Intersection[] LocalIntersect(Ray localRay)
        {
            (var xtmin, var xtmax) = CheckAxis(localRay.Origin.X, localRay.Direction.X);
            (var ytmin, var ytmax) = CheckAxis(localRay.Origin.Y, localRay.Direction.Y);
            (var ztmin, var ztmax) = CheckAxis(localRay.Origin.Z, localRay.Direction.Z);

            var tmin = Math.Max(Math.Max(xtmin, ytmin), ztmin);
            var tmax = Math.Min(Math.Min(xtmax, ytmax), ztmax);

            if (tmin > tmax) return new Intersection[] { };

            return new Intersection[] { 
                new Intersection(tmin, this), 
                new Intersection(tmax, this)
            };
        }

        protected override PointType LocalNormalAt(PointType point)
        {
            var maxC = Math.Max(Math.Max(Math.Abs(point.X), Math.Abs(point.Y)), Math.Abs(point.Z));

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
    }
}
