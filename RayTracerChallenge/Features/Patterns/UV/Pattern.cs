using System;

namespace RayTracerChallenge.Features.Patterns.UV
{
    abstract public class Pattern
    {

        public Color A { get; set; }
        public Color B { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        abstract public Color At(double u, double v);

        static public (double u, double v) SphericalMap(PointType point)
        {
            var theta = Math.Atan2(point.X, point.Z);
            
            var vec = PointType.Vector(point.X, point.Y, point.Z);
            var radius = vec.Magnetude();

            var phi = Math.Acos(point.Y / radius);
            var rawU = theta / (2 * Math.PI);

            var u = 1 - (rawU + 0.5);
            var v = 1 - (phi / Math.PI);

            return (u, v);
        }
        static public (double u, double v) PlanarMap(PointType point)
        {
            var u = (point.X % 1 + 1) % 1;
            var v = (point.Z % 1 + 1) % 1;

            return (u, v);
        }
        static public (double u, double v) CylindricalMap(PointType point)
        {
            var theta = Math.Atan2(point.X, point.Z);
            var rawU = theta / (2 * Math.PI);

            var u = 1 - (rawU + 0.5);
            var v = (point.Y % 1 + 1) % 1;

            return (u, v);
        }
    }
}