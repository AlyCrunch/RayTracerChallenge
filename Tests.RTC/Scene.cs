using System;
using RTF = RayTracerChallenge.Features;
using RTH = RayTracerChallenge.Helpers;
using Tests.RTC.Helpers;
using Xunit;

namespace Tests.RTC
{
    public class Scene
    {
        [Fact]
        public void CreatingWorld()
        {
            var w = new RTF.World();
            Assert.Empty(w.Objects);
            Assert.Null(w.Light);
        }

        [Fact]
        public void TheDefaultWorld()
        {
            var light = new RTH.Light(
                RTF.PointType.Point(-10, 10, -10),
                RTF.Color.White());

            var s1 = new RTF.Sphere(
                        new RTF.Material(
                            new RTF.Color(0.8, 1, 0.6),
                            0.7,
                            0.2
                        )
                    );
            var s2 = new RTF.Sphere(
                    RTH.Transformations.Scaling(0.5, 0.5, 0.5)
                );

            RTF.World w = RTF.World.Default();

            Assert.Equal(light, w.Light);
            Assert.Contains(s1, w.Objects);
            Assert.Contains(s2, w.Objects);
        }

        [Fact]
        public void IntersectWorldWithRay()
        {
            var w = RTF.World.Default();
            var r = new RTF.Ray(
                RTF.PointType.Point(0, 0, -5),
                RTF.PointType.Vector(0, 0, 1));
            RTF.Intersection[] xs = RTF.Intersection.Intersect(w, r);

            Assert.Equal(4, xs.Length);
            Assert.Equal(4, xs[0].T);
            Assert.Equal(4.5, xs[1].T);
            Assert.Equal(5.5, xs[2].T);
            Assert.Equal(6, xs[3].T);
        }

        [Fact]
        public void PrecomputingStateIntersection()
        {
            var r = new RTF.Ray(
                   RTF.PointType.Point(0, 0, -5),
                   RTF.PointType.Vector(0, 0, 1));
            var shape = new RTF.Sphere();
            var i = new RTF.Intersection(4, shape);
            var comps = RTF.Computation.PrepareComputation(i, r);

            Assert.Equal(i.T, comps.T);
            Assert.Equal(i.Object, comps.Object);
            Assert.Equal(RTF.PointType.Point(0, 0, -1), comps.Point);
            Assert.Equal(RTF.PointType.Vector(0, 0, -1), comps.EyeV);
            Assert.Equal(RTF.PointType.Vector(0, 0, -1), comps.NormalV);
        }

        [Fact]
        public void HitWhenIntersectionOccursOutside()
        {
            var r = new RTF.Ray(
                   RTF.PointType.Point(0, 0, -5),
                   RTF.PointType.Vector(0, 0, 1));
            var shape = new RTF.Sphere();
            var i = new RTF.Intersection(4, shape);
            var comps = RTF.Computation.PrepareComputation(i, r);

            Assert.False(comps.Inside);
        }

        [Fact]
        public void HitWhenIntersectionOccursInside()
        {
            var r = new RTF.Ray(
                   RTF.PointType.Point(0, 0, 0),
                   RTF.PointType.Vector(0, 0, 1));
            var shape = new RTF.Sphere();
            var i = new RTF.Intersection(1, shape);
            var comps = RTF.Computation.PrepareComputation(i, r);

            
            Assert.Equal(RTF.PointType.Point(0, 0, 1), comps.Point);
            Assert.Equal(RTF.PointType.Vector(0, 0, -1), comps.EyeV);
            Assert.True(comps.Inside);
            Assert.Equal(RTF.PointType.Vector(0, 0, -1), comps.NormalV);
        }
    }
}
