using System;
using Tests.RTC.Helpers;
using Xunit;
using pt = RayTracerChallenge.Features.PointType;
using shape = RayTracerChallenge.Features.Shapes;
using pattern = RayTracerChallenge.Features.Patterns;
using transform = RayTracerChallenge.Helpers.Transformations;
using c = RayTracerChallenge.Features.Color;
using RayTracerChallenge.Features;
using System.Linq;

namespace Tests.RTC
{
    public class Groups
    {
        [Fact]
        public void CreatingNewGroup()
        {
            var g = new shape.Group();

            Assert.Equal(Matrix.GetIdentity(), g.Transform);
            Assert.True(g.IsEmpty());
        }

        [Fact]
        public void ShapeParentIsNothing()
        {
            var g = new shape.Group();
            Assert.Null(g.Parent);
        }

        [Fact]
        public void AddingChildToGroup()
        {
            var g = new shape.Group();
            var s = new shape.TestShape();
            g.Add(s);

            Assert.NotEmpty(g.Children);
            Assert.Contains(s, g.Children);
            Assert.Equal(g, s.Parent);
        }

        [Fact]
        public void IntersectingRayWithEmptyGroup()
        {
            var g = new shape.Group();
            var r = new Ray(pt.Point(0, 0, 0), pt.Vector(0, 0, 0));
            var xs = g.Intersect(r);
            Assert.Empty(xs);
        }
        [Fact]
        public void IntersectingRayWithNonemptyGroup()
        {
            var g = new shape.Group();
            var s1 = new shape.Sphere();
            var s2 = new shape.Sphere(transform.Translation(0, 0, -3));
            var s3 = new shape.Sphere(transform.Translation(5, 0, 0));
            g.Add(s1);
            g.Add(s2);
            g.Add(s3);

            var r = new Ray(pt.Point(0, 0, -5), pt.Vector(0, 0, 1));
            var xs = g.Intersect(r);
            Assert.Equal(4, xs.Count());
            Assert.Equal(s2, xs[0].Object);
            Assert.Equal(s2, xs[1].Object);
            Assert.Equal(s1, xs[2].Object);
            Assert.Equal(s1, xs[3].Object);
        }
        [Fact]
        public void IntersectingATransformedGroup()
        {
            var g = new shape.Group(transform.Scaling(2, 2, 2));
            var s = new shape.Sphere(transform.Translation(5, 0, 0));
            g.Add(s);

            var r = new Ray(pt.Point(10, 0, -10), pt.Vector(0, 0, 1));
            var xs = g.Intersect(r);
            Assert.Equal(2, xs.Count());
        }

        [Fact]
        public void ConvertingPointFromWorldToObjectSpace()
        {
            var g1 = new shape.Group(transform.RotationY(Math.PI / 2));
            var g2 = new shape.Group(transform.Scaling(2, 2, 2));
            g1.Add(g2);

            var s = new shape.Sphere(transform.Translation(5, 0, 0));
            g2.Add(s);

            var p = s.WorldToObject(pt.Point(-2, 0, -10));

            CustomAssert.Equal(pt.Point(0, 0, -1), p, 5);
        }

        [Fact]
        public void ConvertingNormalFromObjectToWorldSpace()
        {
            var g1 = new shape.Group(transform.RotationY(Math.PI / 2));
            var g2 = new shape.Group(transform.Scaling(1, 2, 3));
            g1.Add(g2);
            var s = new shape.Sphere(transform.Translation(5, 0, 0));
            g2.Add(s);
            var value = Math.Sqrt(3) / 3;
            var n = s.NormalToWorld(pt.Vector(value, value, value));
            CustomAssert.Equal(pt.Vector(0.2857, 0.4286, -0.8571), n, 4);
        }

        [Fact]
        public void FindingNormalOnChildObject()
        {
            var g1 = new shape.Group(transform.RotationY(Math.PI / 2));
            var g2 = new shape.Group(transform.Scaling(1, 2, 3));
            g1.Add(g2);
            var s = new shape.Sphere(transform.Translation(5, 0, 0));
            g2.Add(s);
            var n = s.NormalAt(pt.Point(1.7311, 1.1547, -5.5774));
            //TODO precision error
            CustomAssert.Equal(pt.Vector(0.2856, 0.4286, -0.8572), n, 4);
        }
    }
}