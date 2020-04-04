using Xunit;
using RTF = RayTracerChallenge.Features;
using RTH = RayTracerChallenge.Helpers;

namespace Tests.RTC
{
    public class RaySphereIntersections
    {
        [Fact]
        public void TransformationSequence()
        {
            var origin = RTF.PointType.Point(1, 2, 3);
            var direction = RTF.PointType.Vector(4, 5, 6);
            var r = new RTF.Ray(origin, direction);

            Assert.Equal(origin, r.Origin);
            Assert.Equal(direction, r.Direction);
        }

        [Fact]
        public void ComputePointFromDistance()
        {
            var origin = RTF.PointType.Point(2, 3, 4);
            var direction = RTF.PointType.Vector(1, 0, 0);
            var r = new RTF.Ray(origin, direction);

            var e1 = RTF.PointType.Point(2, 3, 4);
            var e2 = RTF.PointType.Point(3, 3, 4);
            var e3 = RTF.PointType.Point(1, 3, 4);
            var e4 = RTF.PointType.Point(4.5, 3, 4);

            Assert.Equal(e1, RTH.Transformations.Position(r, 0));
            Assert.Equal(e2, RTH.Transformations.Position(r, 1));
            Assert.Equal(e3, RTH.Transformations.Position(r, -1));
            Assert.Equal(e4, RTH.Transformations.Position(r, 2.5));
        }

        [Fact]
        public void RaySphereTwoPoints()
        {
            var origin = RTF.PointType.Point(0, 0, -5);
            var direction = RTF.PointType.Vector(0, 0, 1);
            var r = new RTF.Ray(origin, direction);
            var s = new RTF.Sphere();

            var xs = RTH.Transformations.Intersect(s, r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(4, xs[0].T);
            Assert.Equal(6, xs[1].T);
        }
        [Fact]
        public void RaySphereTangent()
        {
            var origin = RTF.PointType.Point(0, 1, -5);
            var direction = RTF.PointType.Vector(0, 0, 1);
            var r = new RTF.Ray(origin, direction);
            var s = new RTF.Sphere();

            var xs = RTH.Transformations.Intersect(s, r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(5, xs[0].T);
            Assert.Equal(5, xs[1].T);
        }
        [Fact]
        public void RaySphereMiss()
        {
            var origin = RTF.PointType.Point(0, 2, -5);
            var direction = RTF.PointType.Vector(0, 0, 1);
            var r = new RTF.Ray(origin, direction);
            var s = new RTF.Sphere();

            var xs = RTH.Transformations.Intersect(s, r);

            Assert.Empty(xs);
        }
        [Fact]
        public void RaySphereInside()
        {
            var origin = RTF.PointType.Point(0, 0, 0);
            var direction = RTF.PointType.Vector(0, 0, 1);
            var r = new RTF.Ray(origin, direction);
            var s = new RTF.Sphere();

            var xs = RTH.Transformations.Intersect(s, r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(-1, xs[0].T);
            Assert.Equal(1, xs[1].T);
        }
        [Fact]
        public void RaySphereBehind()
        {
            var origin = RTF.PointType.Point(0, 0, 5);
            var direction = RTF.PointType.Vector(0, 0, 1);
            var r = new RTF.Ray(origin, direction);
            var s = new RTF.Sphere();

            var xs = RTH.Transformations.Intersect(s, r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(-6, xs[0].T);
            Assert.Equal(-4, xs[1].T);
        }

        [Fact]
        public void IntersectionEncapsulatesTAndObject()
        {
            var s = new RTF.Sphere();
            var i = new RTF.Intersection(3.5, s);

            Assert.Equal(3.5, i.T);
            Assert.Equal(s, i.Object);
        }
        [Fact]
        public void AggregatingIntersections()
        {
            var s = new RTF.Sphere();
            var i1 = new RTF.Intersection(1, s);
            var i2 = new RTF.Intersection(2, s);

            var xs = RTF.Intersection.Intersections(i1, i2);

            Assert.Equal(2, xs.Length);
            Assert.Equal(1, xs[0].T);
            Assert.Equal(2, xs[1].T);
        }
        [Fact]
        public void SetObjectIntersection()
        {
            var r = new RTF.Ray(RTF.PointType.Point(0, 0, -5),
                RTF.PointType.Vector(0, 0, 1));
            var s = new RTF.Sphere();
            var xs = RTF.Intersection.Intersect(s, r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(s, xs[0].Object);
            Assert.Equal(s, xs[1].Object);
        }

        [Fact]
        public void HitAllIntersectionPositiveT()
        {
            var s = new RTF.Sphere();
            var i1 = new RTF.Intersection(1, s);
            var i2 = new RTF.Intersection(2, s);
            var xs = RTF.Intersection.Intersections(i2, i1);

            RTF.Intersection i = RTF.Intersection.Hit(xs);

            Assert.Equal(i1, i);
        }
        [Fact]
        public void HitSomeIntersectionNegativeT()
        {
            var s = new RTF.Sphere();
            var i1 = new RTF.Intersection(-1, s);
            var i2 = new RTF.Intersection(1, s);
            var xs = RTF.Intersection.Intersections(i2, i1);

            RTF.Intersection i = RTF.Intersection.Hit(xs);

            Assert.Equal(i2, i);
        }
        [Fact]
        public void HitAllIntersectionNegativeT()
        {
            var s = new RTF.Sphere();
            var i1 = new RTF.Intersection(-2, s);
            var i2 = new RTF.Intersection(-1, s);
            var xs = RTF.Intersection.Intersections(i2, i1);

            var i = RTF.Intersection.Hit(xs);

            Assert.Null(i);
        }
        [Fact]
        public void HitAlwaysLowestNonNegativeIntersec()
        {
            var s = new RTF.Sphere();
            var i1 = new RTF.Intersection(5, s);
            var i2 = new RTF.Intersection(7, s);
            var i3 = new RTF.Intersection(-3, s);
            var i4 = new RTF.Intersection(2, s);
            var xs = RTF.Intersection.Intersections(i1, i2, i3, i4);
            RTF.Intersection i = RTF.Intersection.Hit(xs);

            Assert.Equal(i4, i);
        }

        [Fact]
        public void TranslatingRay()
        {
            var r = new RTF.Ray(RTF.PointType.Point(1, 2, 3),
                                RTF.PointType.Vector(0, 1, 0));
            var m = RTH.Transformations.Translation(3, 4, 5);

            RTF.Ray r2 = RTF.Ray.Transform(r, m);

            Assert.Equal(RTF.PointType.Point(4, 6, 8), r2.Origin);
            Assert.Equal(RTF.PointType.Vector(0, 1, 0), r2.Direction);
        }
        [Fact]
        public void ScalingRay()
        {
            var r = new RTF.Ray(RTF.PointType.Point(1, 2, 3),
                                RTF.PointType.Vector(0, 1, 0));
            var m = RTH.Transformations.Scaling(2, 3, 4);

            RTF.Ray r2 = RTF.Ray.Transform(r, m);

            Assert.Equal(RTF.PointType.Point(2, 6, 12), r2.Origin);
            Assert.Equal(RTF.PointType.Vector(0, 3, 0), r2.Direction);
        }

        [Fact]
        public void SphereDefaultTransformation()
        {
            var s = new RTF.Sphere();
            Assert.Equal(RTF.Matrix.GetIdentity(4, 4), s.Transform);
        }
        [Fact]
        public void ChangingSphereTransformation()
        {
            var s = new RTF.Sphere();
            var t = RTH.Transformations.Translation(2, 3, 4);
            s.Transform = t;

            Assert.Equal(t, s.Transform);
        }
        [Fact]
        public void IntersectingScaledSphereWithRay()
        {
            var s = new RTF.Sphere();
            var r = new RTF.Ray(RTF.PointType.Point(0, 0, -5),
                                RTF.PointType.Vector(0, 0, 1));

            s.Transform = RTH.Transformations.Scaling(2, 2, 2);
            var xs = RTF.Intersection.Intersect(s, r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(3, xs[0].T);
            Assert.Equal(7, xs[1].T);
        }
        [Fact]
        public void IntersectTranslatedSphereWithRay()
        {
            var s = new RTF.Sphere();
            var r = new RTF.Ray(RTF.PointType.Point(0, 0, -5),
                                RTF.PointType.Vector(0, 0, 1));
            s.Transform = RTH.Transformations.Translation(5, 0, 0);
            var xs = RTF.Intersection.Intersect(s, r);

            Assert.Empty(xs);
        }
    }
}
