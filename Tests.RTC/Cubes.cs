using System;
using Tests.RTC.Helpers;
using Xunit;
using pt = RayTracerChallenge.Features.PointType;
using shape = RayTracerChallenge.Features.Shapes;
using pattern = RayTracerChallenge.Features.Patterns;
using transform = RayTracerChallenge.Helpers.Transformations;
using c = RayTracerChallenge.Features.Color;
using RayTracerChallenge.Features;

namespace Tests.RTC
{
    public class Cubes
    {
        [Theory]
        [InlineData(5, 0.5, 0, -1, 0, 0, 4, 6)]
        [InlineData(-5, 0.5, 0, 1, 0, 0, 4, 6)]
        [InlineData(0.5, 5, 0, 0, -1, 0, 4, 6)]
        [InlineData(0.5, -5, 0, 0, 1, 0, 4, 6)]
        [InlineData(0.5, 0, 5, 0, 0, -1, 4, 6)]
        [InlineData(0.5, 0, -5, 0, 0, 1, 4, 6)]
        [InlineData(0, 0.5, 0, 0, 0, 1, -1, 1)]
        public void RayIntersectCube(double px, double py, double pz, double vx, double vy, double vz, double t1, double t2)
        {
            var point = pt.Point(px, py, pz);
            var direction = pt.Vector(vx, vy, vz);

            var c = new shape.Cube();
            var r = new Ray(point, direction);

            var xs = c.Intersect(r);
            Assert.Equal(2, xs.Length);
            Assert.Equal(t1, xs[0].T);
            Assert.Equal(t2, xs[1].T);
        }

        [Theory]
        [InlineData(-2, 0, 0, 0.2673, 0.5343, 0.8018)]
        [InlineData(0, -2, 0, 0.8018, 0.2673, 0.5345)]
        [InlineData(0, 0, -2, 0.5345, 0.8018, 0.2673)]
        [InlineData(2, 0, 2, 0, 0, -1)]
        [InlineData(0, 2, 2, 0, -1, 0)]
        [InlineData(2, 2, 0, -1, 0, 0)]
        public void RayMissCube(double px, double py, double pz, double vx, double vy, double vz)
        {
            var point = pt.Point(px, py, pz);
            var direction = pt.Vector(vx, vy, vz);

            var c = new shape.Cube();
            var r = new Ray(point, direction);

            var xs = c.Intersect(r);
            Assert.Empty(xs);
        }

        [Theory]
        [InlineData(1, 0.5, -0.8, 1, 0, 0)]
        [InlineData(-1, -0.2, 0.9, -1, 0, 0)]
        [InlineData(-0.4, 1, -0.1, 0, 1, 0)]
        [InlineData(0.3, -1, -0.7, 0, -1, 0)]
        [InlineData(-0.6, 0.3, 1, 0, 0, 1)]
        [InlineData(0.4, 0.4, -1, 0, 0, -1)]
        [InlineData(1, 1, 1, 1, 0, 0)]
        [InlineData(-1, -1, -1, -1, 0, 0)]
        public void NormalSurfaceCube(double px, double py, double pz, double vx, double vy, double vz)
        {
            var p = pt.Point(px, py, pz);
            var expNormal = pt.Vector(vx, vy, vz);

            var c = new shape.Cube();
            var normal = c.NormalAt(p);
            Assert.Equal(expNormal, normal);
        }

    }
}
