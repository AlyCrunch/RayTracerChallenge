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
using System.Collections.Generic;

namespace Tests.RTC
{
    public class ConstructiveSolidGeometry
    {
        [Fact]
        public void CSGCreatedWithOperationAndTwoShapes()
        {
            var s1 = new shape.Sphere();
            var s2 = new shape.Cube();
            var c = new shape.CSG(shape.Operation.Union, s1, s2);

            Assert.Equal(shape.Operation.Union, c.Operation);
            Assert.Equal(s1, c.Left);
            Assert.Equal(s2, c.Right);
            Assert.Equal(c, s1.Parent);
            Assert.Equal(c, s2.Parent);
        }
        [Theory]
        [InlineData("Union", true, true, true, false)]
        [InlineData("Union", true, true, false, true)]
        [InlineData("Union", true, false, true, false)]
        [InlineData("Union", true, false, false, true)]
        [InlineData("Union", false, true, true, false)]
        [InlineData("Union", false, true, false, false)]
        [InlineData("Union", false, false, true, true)]
        [InlineData("Union", false, false, false, true)]
        [InlineData("Intersection", true, true, true, true)]
        [InlineData("Intersection", true, true, false, false)]
        [InlineData("Intersection", true, false, true, true)]
        [InlineData("Intersection", true, false, false, false)]
        [InlineData("Intersection", false, true, true, true)]
        [InlineData("Intersection", false, true, false, true)]
        [InlineData("Intersection", false, false, true, false)]
        [InlineData("Intersection", false, false, false, false)]
        [InlineData("Difference", true, true, true, false)]
        [InlineData("Difference", true, true, false, true)]
        [InlineData("Difference", true, false, true, false)]
        [InlineData("Difference", true, false, false, true)]
        [InlineData("Difference", false, true, true, true)]
        [InlineData("Difference", false, true, false, true)]
        [InlineData("Difference", false, false, true, false)]
        [InlineData("Difference", false, false, false, false)]
        public void EvaluatingRule(string op, bool lhit, bool inl, bool inr, bool result)
        {
            var operation = (shape.Operation)Enum.Parse(typeof(shape.Operation), op);
            var r = shape.CSG.IntersectionAllowed(operation, lhit, inl, inr);
            Assert.Equal(result, r);
        }

        [Theory]
        [InlineData("Union", 0, 3)]
        [InlineData("Intersection", 1, 2)]
        [InlineData("Difference", 0, 1)]
        public void FilteringListIntersections(string op, int x0, int x1)
        {
            var operation = (shape.Operation)Enum.Parse(typeof(shape.Operation), op);
            var s1 = new shape.Sphere();
            var s2 = new shape.Cube();

            var c = new shape.CSG(operation, s1, s2);
            var xs = Intersection.Intersections(
                new Intersection(1, s1),
                new Intersection(2, s2),
                new Intersection(3, s1),
                new Intersection(4, s2)
                );

            List<Intersection> result = c.FilterIntersections(xs);
            Assert.Equal(2, result.Count);
            Assert.Equal(xs[x0], result[0]);
            Assert.Equal(xs[x1], result[1]);
        }

        [Fact]
        public void RayMissesCSGObject()
        {
            var c = new shape.CSG(shape.Operation.Union, new shape.Sphere(), new shape.Cube());
            var r = new Ray(pt.Point(0, 2, -5), pt.Vector(0, 0, 1));
            var xs = c.Intersect(r);

            Assert.Empty(xs);
        }
        [Fact]
        public void RayHitsCSGObject()
        {
            var s1 = new shape.Sphere();
            var s2 = new shape.Sphere(transform.Translation(0, 0, 0.5));
            var c = new shape.CSG(shape.Operation.Union, s1, s2);
            var r = new Ray(pt.Point(0, 0, -5), pt.Vector(0, 0, 1));
            var xs = c.Intersect(r);

            Assert.Equal(2, xs.Count());
            Assert.Equal(4, xs[0].T);
            Assert.Equal(s1, xs[0].Object);
            Assert.Equal(6.5, xs[1].T);
            Assert.Equal(s2, xs[1].Object);
        }
    }
}
