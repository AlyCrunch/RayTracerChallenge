using System;
using System.Collections.Generic;
using pt = RayTracerChallenge.Features.PointType;

namespace RayTracerChallenge.Features.Shapes
{
    public class Torus : Shape
    {
        const double EPSILON = 0.00001;

        public double MajorR { get; set; }
        public double TubeR { get; set; }

        public override BoundingBox Bounds()
        {
            return new BoundingBox(
                pt.Point(-(MajorR + TubeR), -TubeR, -(MajorR + TubeR)),
                pt.Point((MajorR + TubeR), TubeR, (MajorR + TubeR))
                );
        }

        protected override Intersection[] LocalIntersect(Ray ray)
        {
            if (!Bounds().Intersects(ray)) return new Intersection[] { };
            (var ox, var oy, var oz, _) = ray.Origin.ToTuple();
            (var dx, var dy, var dz, _) = ray.Direction.ToTuple();

            var sumDsqrd = dx * dx + dy * dy + dz * dz;
            var e = ox * ox + oy * oy + oz * oz + TubeR * TubeR + MajorR * MajorR;
            var f = ox * dx + oy * dy + oz * dy;
            var fourASqrd = 4 * MajorR * MajorR;

            var coeffs = new double[]
            {
                e * e - fourASqrd * (TubeR*TubeR - oy*oy),
                4 * f * e + 2 * fourASqrd * oy * dy,
                2 * sumDsqrd * e + 4 * f * f + fourASqrd * dy * dy,
                4* sumDsqrd * f,
                sumDsqrd * sumDsqrd
            };


            return new Intersection[] { };
        }

        protected override PointType LocalNormalAt(PointType point, Intersection hit = null)
        {
            throw new NotImplementedException();
        }
    }
}
