using Tests.RTC.Helpers;
using Xunit;
using pt = RayTracerChallenge.Features.PointType;
using shape = RayTracerChallenge.Features.Shapes;
using RayTracerChallenge.Features;

namespace Tests.RTC
{
    public class Cylinders
    {
        [Theory]
        [InlineData(1, 0, 0, 0, 1, 0)]
        [InlineData(0, 0, 0, 0, 1, 0)]
        [InlineData(0, 0, -5, 1, 1, 1)]
        public void RayMissCylinder(double pX, double pY, double pZ, double vX, double vY, double vZ)
        {
            var origin = pt.Point(pX, pY, pZ);
            var direction = pt.Vector(vX, vY, vZ).Normalize();
            var cyl = new shape.Cylinder();
            var ray = new Ray(origin, direction);
            var xs = cyl.Intersect(ray);

            Assert.Empty(xs);
        }

        [Theory]
        [InlineData(1, 0, -5, 0, 0, 1, 5, 5)]
        [InlineData(0, 0, -5, 0, 0, 1, 4, 6)]
        [InlineData(0.5, 0, -5, 0.1, 1, 1, 6.80798, 7.08872)]
        public void RayStrikeCylinder(double pX, double pY, double pZ, double vX, double vY, double vZ, double t0, double t1)
        {
            var cyl = new shape.Cylinder();

            var origin = pt.Point(pX, pY, pZ);
            var direction = pt.Vector(vX, vY, vZ).Normalize();

            var ray = new Ray(origin, direction);
            var xs = cyl.Intersect(ray);

            Assert.Equal(2, xs.Length);
            Assert.Equal(t0, xs[0].T, 5);
            Assert.Equal(t1, xs[1].T, 5);
        }

        [Theory]
        [InlineData(1, 0, 0, 1, 0, 0)]
        [InlineData(0, 5, -1, 0, 0, -1)]
        [InlineData(0, -2, 1, 0, 0, 1)]
        [InlineData(-1, 1, 0, -1, 0, 0)]
        public void NormalOnCylinder(double pX, double pY, double pZ, double vX, double vY, double vZ)
        {
            var point = pt.Point(pX, pY, pZ);
            var normal = pt.Vector(vX, vY, vZ);
            var cyl = new shape.Cylinder();
            var n = cyl.NormalAt(point);

            Assert.Equal(normal, n);
        }

        [Fact]
        public void DefaultMinAndMaxCylinder()
        {
            var cyl = new shape.Cylinder();
            Assert.True(double.IsNegativeInfinity(cyl.Minimum));
            Assert.True(double.IsPositiveInfinity(cyl.Maximum));
        }

        [Theory]
        [InlineData(0, 1.5, 0, 0.1, 1, 0, 0)]
        [InlineData(0, 3, -5, 0, 0, 1, 0)]
        [InlineData(0, 0, -5, 0, 0, 1, 0)]
        [InlineData(0, 2, -5, 0, 0, 1, 0)]
        [InlineData(0, 1, -5, 0, 0, 1, 0)]
        [InlineData(0, 1.5, -2, 0, 0, 1, 2)]
        public void IntersectingConstrainedCylinder(double pX, double pY, double pZ, double vX, double vY, double vZ, int count)
        {
            var point = pt.Point(pX, pY, pZ);
            var direction = pt.Vector(vX, vY, vZ).Normalize();

            var cyl = new shape.Cylinder
            {
                Minimum = 1,
                Maximum = 2
            };

            var r = new Ray(point, direction);
            var xs = cyl.Intersect(r);
            Assert.Equal(count, xs.Length);
        }

        [Fact]
        public void DefaultClosedValueCylinder()
        {
            var cyl = new shape.Cylinder();
            Assert.False(cyl.Closed);
        }

        [Theory]
        [InlineData(0, 3, 0, 0, -1, 0, 2)]
        [InlineData(0, 3, -2, 0, -1, 2, 2)]
        [InlineData(0, 4, -2, 0, -1, 1, 2)]
        [InlineData(0, 0, -2, 0, 1, 2, 2)]
        [InlineData(0, -1, -2, 0, 1, 1, 2)]
        public void IntersectingCapsClosedCylinder(double pX, double pY, double pZ, double vX, double vY, double vZ, int count)
        {
            var point = pt.Point(pX, pY, pZ);
            var direction = pt.Vector(vX, vY, vZ).Normalize();

            var cyl = new shape.Cylinder
            {
                Minimum = 1,
                Maximum = 2,
                Closed = true
            };

            var r = new Ray(point, direction);
            var xs = cyl.Intersect(r);
            Assert.Equal(count, xs.Length);
        }

        [Theory]
        [InlineData(0, 1, 0, 0, -1, 0)]
        [InlineData(0.5, 1, 0, 0, -1, 0)]
        [InlineData(0, 1, 0.5, 0, -1, 0)]
        [InlineData(0, 2, 0, 0, 1, 0)]
        [InlineData(0.5, 2, 0, 0, 1, 0)]
        [InlineData(0, 2, 0.5, 0, 1, 0)]
        public void NormalOnCylinderEndCap(double pX, double pY, double pZ, double vX, double vY, double vZ)
        {
            var point = pt.Point(pX, pY, pZ);
            var normal = pt.Vector(vX, vY, vZ);
            var cyl = new shape.Cylinder()
            {
                Minimum = 1,
                Maximum = 2,
                Closed = true
            };
            var n = cyl.NormalAt(point);

            Assert.Equal(normal, n);
        }

        [Theory]
        [InlineData(0, 0, -5, 0, 0, 1, 5, 5)]
        [InlineData(0, 0, -5, 1, 1, 1, 8.66025, 8.66025)]
        [InlineData(1, 1, -5, -0.5, -1, 1, 4.55006, 49.44994)]
        public void IntersectConeRay(double pX, double pY, double pZ, double vX, double vY, double vZ, double t0, double t1)
        {
            var s = new shape.Cone();

            var origin = pt.Point(pX, pY, pZ);
            var direction = pt.Vector(vX, vY, vZ).Normalize();

            var ray = new Ray(origin, direction);
            var xs = s.Intersect(ray);

            Assert.Equal(2, xs.Length);
            Assert.Equal(t0, xs[0].T, 5);
            Assert.Equal(t1, xs[1].T, 5);
        }

        [Fact]
        public void IntersectingConeWithRayParallelOneHalves()
        {
            var s = new shape.Cone();
            var direction = pt.Vector(0, 1, 1).Normalize();
            var r = new Ray(pt.Point(0, 0, -1), direction);
            var xs = s.Intersect(r);

            Assert.Single(xs);
            Assert.Equal(0.35355, xs[0].T, 5);
        }

        [Theory]
        [InlineData(0, 0, -5, 0, 1, 0, 0)]
        [InlineData(0, 0, -0.25, 0, 1, 1, 2)]
        [InlineData(0, 0, -0.25, 0, 1, 0, 4)]
        public void IntersectConeEndCap(double pX, double pY, double pZ, double vX, double vY, double vZ, double count)
        {
            var s = new shape.Cone()
            {
                Minimum = -0.5,
                Maximum = 0.5,
                Closed = true
            };

            var origin = pt.Point(pX, pY, pZ);
            var direction = pt.Vector(vX, vY, vZ).Normalize();

            var ray = new Ray(origin, direction);
            var xs = s.Intersect(ray);

            Assert.Equal(count, xs.Length);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 1, -1.41421356237, 1)]
        [InlineData(-1, -1, 0, -1, 1, 0)]
        public void ComputingNormalVectorCone(double pX, double pY, double pZ, double vX, double vY, double vZ)
        {
            var s = new shape.Cone();

            var point = pt.Point(pX, pY, pZ);
            var normal = pt.Vector(vX, vY, vZ).Normalize();

            var n = s.NormalAt(point);

            CustomAssert.Equal(normal, n, 5);
        }
    }
}