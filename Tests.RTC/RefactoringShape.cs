using System;
using RTF = RayTracerChallenge.Features;
using pt = RayTracerChallenge.Features.PointType;
using tf = RayTracerChallenge.Helpers.Transformations;
using shapes = RayTracerChallenge.Features.Shapes;
using Tests.RTC.Helpers;
using Xunit;

namespace Tests.RTC
{
    public class RefactoringShape
    {
        [Fact]
        public void DefaultTransformation()
        {
            var s = new shapes.TestShape();
            var identity = RTF.Matrix.GetIdentity();

            Assert.Equal(identity, s.Transform);
        }

        [Fact]
        public void AssigningTransformation()
        {
            var t = tf.Translation(2, 3, 4);
            var s = new shapes.TestShape(t);

            Assert.Equal(t, s.Transform);
        }

        [Fact]
        public void DefaultMaterial()
        {
            var s = new shapes.TestShape();
            Assert.Equal(new RTF.Material(), s.Material);
        }
        [Fact]
        public void AssigningMaterial()
        {
            var m = new RTF.Material
            {
                Ambient = 1
            };
            var s = new shapes.TestShape(m);

            Assert.Equal(m, s.Material);
        }

        [Fact]
        public void IntersectingScaledShapeWithRay()
        {
            var r = new RTF.Ray(pt.Point(0, 0, -5), pt.Vector(0, 0, 1));
            var s = new shapes.TestShape(tf.Scaling(2, 2, 2));

            _ = s.Intersect(r);

            Assert.Equal(pt.Point(0, 0, -2.5), s.SavedRay.Origin);
            Assert.Equal(pt.Vector(0, 0, 0.5), s.SavedRay.Direction);
        }
        [Fact]
        public void IntersectingTranslatedShapeWithRay()
        {
            var r = new RTF.Ray(pt.Point(0, 0, -5), pt.Vector(0, 0, 1));
            var s = new shapes.TestShape(tf.Translation(5, 0, 0));

            _ = s.Intersect(r);

            Assert.Equal(pt.Point(-5, 0, -5), s.SavedRay.Origin);
            Assert.Equal(pt.Vector(0, 0, 1), s.SavedRay.Direction);
        }

        [Fact]
        public void ComputingNormalOnTranslatedShape()
        {
            var s = new shapes.TestShape(tf.Translation(0, 1, 0));
            var n = s.NormalAt(pt.Point(0, 1.70711, -0.70711));
            var exp = pt.Vector(0, 0.70711, -0.70711);

            CustomAssert.Equal(exp, n, 5);
        }
        [Fact]
        public void ComputingNormalOnTransformedShape()
        {
            var s = new shapes.TestShape(tf.Scaling(1, 0.5, 1) * tf.RotationZ(Math.PI / 5));
            var n = s.NormalAt(pt.Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2));
            var exp = pt.Vector(0, 0.97014, -0.24254);

            CustomAssert.Equal(exp, n, 5);
        }
    }
}
